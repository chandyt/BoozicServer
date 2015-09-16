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


            String UPCAPIKey = appSettingsService.GetUPCAPIKey();

            HttpClient httpUPCClient = new HttpClient();

            HttpResponseMessage UPC_Response = httpUPCClient.GetAsync("http://api.upcdatabase.org/json/" + UPCAPIKey + "/" + UPC).Result;
            if (UPC_Response.IsSuccessStatusCode)
            {

                string upcResult = UPC_Response.Content.ReadAsStringAsync().Result.ToString();
                // parsing Data out of JSON result from google places API
                JObject json = Newtonsoft.Json.Linq.JObject.Parse(upcResult);
                string productName = json["itemname"].ToString();
                if (productName == string.Empty)
                {
                     productName = json["description"].ToString();
                }
                return productName;
            }

            return "Test";
        }
    }
}