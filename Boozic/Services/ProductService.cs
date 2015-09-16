using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Boozic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository repository;
        private readonly ISettingsService appSettingsService;
        public ProductService(IProductsRepository aRepository)
        {
            repository = aRepository;
            appSettingsService = new SettingsService(new SettingsRepository(new BoozicEntities()));
        }

    


        public Product GetById(int aProductID)
        {
          return  repository.GetById(aProductID);
        }
        public Models.ProductInfo GetByUPC(string aUPC)
        {
            return repository.GetByUPC(aUPC);

        }

        public string getProductName(string UPC)
        {

            // Generating Parameters for Google Places API
            NameValueCollection UPC_Details = new NameValueCollection();
            String UPCAPIKey = appSettingsService.GetUPCAPIKey();

            HttpClient httpUPCClient = new HttpClient();
            //httpGPClient.BaseAddress = new Uri("http://api.upcdatabase.org/json/" + UPCAPIKey + "/" + UPC);
            HttpResponseMessage GP_Response = httpUPCClient.GetAsync("http://api.upcdatabase.org/json/" + UPCAPIKey + "/" + UPC).Result;
            if (GP_Response.IsSuccessStatusCode)
            {

                string upcResult = GP_Response.Content.ReadAsStringAsync().Result.ToString();
                // parsing Data out of JSON result from google places API
                JObject json = Newtonsoft.Json.Linq.JObject.Parse(upcResult);
                string productName = json["itemname"].ToString();
                if (productName == string.Empty)
                {
                     productName = json["description"].ToString();
                }
                return productName;
                
                //for (int i = 0; i < results.Count; i++)
              //  {
                    //StoreInfo si = new StoreInfo();
                    //JToken location = json["results"][i]["geometry"]["location"];
                    //si.Latitude = location["lat"].Value<double>();
                    //si.Longitude = location["lng"].Value<double>();

                    //si.StoreName = json["results"][i]["name"].Value<string>();
                    //si.StoreAddress = json["results"][i]["vicinity"].Value<string>();

                    //JToken hours = json["results"][i]["opening_hours"];
                    //if (hours != null)
                    //    si.IsOpenNow = hours["open_now"].Value<bool>();

                    // Generating parameters for Google Matrix API to find the distance and the duration
                    NameValueCollection GM_Details = new NameValueCollection();
                    //GM_Details.Add("key", googleAPIKey);
                    //GM_Details.Add("origins", Latitude + "," + Longitude);
                    //GM_Details.Add("destinations", si.Latitude + "," + si.Longitude);
                    //GM_Details.Add("units", "imperial");

                    //string GM_Url = "json?" + string.Join("&", GM_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GM_Details[x]))));

                    //HttpClient httpGMClient = new HttpClient();
                    //httpGMClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/");
                    //HttpResponseMessage GM_Response = httpGMClient.GetAsync(GM_Url).Result;
                    //if (GM_Response.IsSuccessStatusCode)
                    //{
                    //    //Parsing out the distance and duration out of the JSON from Google Matrix API.
                    //    string googleMatrixResult = GM_Response.Content.ReadAsStringAsync().Result.ToString();

                    //    JObject GM_Json = Newtonsoft.Json.Linq.JObject.Parse(googleMatrixResult);
                    //    JArray rows = (JArray)GM_Json["rows"];
                    //    if (rows.Count > 0)
                    //    {
                    //        si.Distance = GM_Json["rows"][0]["elements"][0]["distance"]["value"].Value<double>();
                    //        si.Duration = GM_Json["rows"][0]["elements"][0]["duration"]["value"].Value<double>();
                    //    }
                    //}

                    //lstSI.Add(si);
               // }



            }

            return "Test";
        }
    }
}