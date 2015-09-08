using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Boozic.Models;
using System.Collections.Specialized;
using System.Web;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boozic.Controllers
{
    public class LocationController : ApiController
    {
        /// <summary>
        /// API function for getting the Liquor store where the User is
        /// </summary>
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <returns>XML data about the store</returns>

        [HttpGet]
        public IHttpActionResult getStoreInfo(double Latitude, double Longitude)
        {
            StoreInfo SI=new StoreInfo();
            List<StoreInfo> lstSI = new List<StoreInfo>();
            lstSI = getStores(Latitude, Longitude, 0.1); // Radius is ~150 feet. to accomadate if the user in parking lots...
            if (lstSI.Count > 0)
                SI = lstSI[0];

            return Ok(SI);
        }

        /// <summary>
        /// API function for getting a List of Liquor stores in the radius
        /// </summary>
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <param name="Radius">Radius in Miles</param>
        /// <returns>XML data about the stores</returns>
         [HttpGet]
        public IHttpActionResult getStoresInRadius(double Latitude, double Longitude, int Radius)
        {

            List<StoreInfo> lstSI = new List<StoreInfo>();
            lstSI = getStores(Latitude, Longitude, Radius);
            return Ok(lstSI);
        }

        /// <summary>
        /// This Function generates a list of StoreInfo objects.
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <param name="Radius">Radius in Miles</param>
        /// <returns> List of StoreInfo Objects</returns>
         private List<StoreInfo> getStores(double Latitude, double Longitude, double Radius)
         {
             List<StoreInfo> lstSI = new List<StoreInfo>();

             // Generating Parameters for Google Places API
             NameValueCollection GP_Details = new NameValueCollection();
             GP_Details.Add("key", "AIzaSyDToe2JDLDLKO_FGVDZSaqJULJ7lbscKrA");
             GP_Details.Add("sensor", "false");
             GP_Details.Add("location", Latitude + "," + Longitude);
             GP_Details.Add("radius", Convert.ToString(Radius * 1609)); // Radius is converted to meters for the API
             GP_Details.Add("types", "liquor_store");

             string GP_Url = "json?" + string.Join("&", GP_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GP_Details[x]))));

             HttpClient httpGPClient = new HttpClient();
             httpGPClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/place/search/");
             HttpResponseMessage GP_Response = httpGPClient.GetAsync(GP_Url).Result;
             if (GP_Response.IsSuccessStatusCode)
             {

                 string googleResult = GP_Response.Content.ReadAsStringAsync().Result.ToString();
                // parsing Data out of JSON result from google places API
                 JObject json = Newtonsoft.Json.Linq.JObject.Parse(googleResult);
                 JArray results = (JArray)json["results"];
                 for (int i = 0; i < results.Count; i++)
                 {
                     StoreInfo si = new StoreInfo();
                     JToken location = json["results"][i]["geometry"]["location"];
                     si.Latitude = location["lat"].Value<double>();
                     si.Longitude = location["lng"].Value<double>();

                     si.StoreName = json["results"][i]["name"].Value<string>();
                     si.StoreAddress = json["results"][i]["vicinity"].Value<string>();

                     JToken hours = json["results"][i]["opening_hours"];
                     if (hours != null)
                         si.IsOpenNow = hours["open_now"].Value<bool>();

                     // Generating parameters for Google Matrix API to find the distance and the duration
                     NameValueCollection GM_Details = new NameValueCollection();
                     GM_Details.Add("key", "AIzaSyDToe2JDLDLKO_FGVDZSaqJULJ7lbscKrA");
                     GM_Details.Add("origins", Latitude + "," + Longitude);
                     GM_Details.Add("destinations", si.Latitude + "," + si.Longitude);
                     GM_Details.Add("units", "imperial");

                     string GM_Url = "json?" + string.Join("&", GM_Details.AllKeys.Select(x => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(GM_Details[x]))));

                     HttpClient httpGMClient = new HttpClient();
                     httpGMClient.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/distancematrix/");
                     HttpResponseMessage GM_Response = httpGMClient.GetAsync(GM_Url).Result;
                     if (GM_Response.IsSuccessStatusCode)
                     {
                         //Parsing out the distance and duration out of the JSON from Google Matrix API.
                         string googleMatrixResult = GM_Response.Content.ReadAsStringAsync().Result.ToString();

                         JObject GM_Json = Newtonsoft.Json.Linq.JObject.Parse(googleMatrixResult);
                         JArray rows = (JArray)GM_Json["rows"];
                         if (rows.Count > 0)
                         {
                             si.Distance = GM_Json["rows"][0]["elements"][0]["distance"]["value"].Value<double>();
                             si.Duration = GM_Json["rows"][0]["elements"][0]["duration"]["value"].Value<double>();
                         }
                     }

                     lstSI.Add(si);
                 }



             }

             return lstSI;
         }
    }
}