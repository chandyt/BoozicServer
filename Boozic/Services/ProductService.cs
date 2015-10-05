using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CookComputing.XmlRpc;

namespace Boozic.Services
{
    [XmlRpcUrl("http://www.upcdatabase.com/xmlrpc")]
    public interface IUPCDatabase : IXmlRpcProxy
    {
        [XmlRpcMethod("lookup")]
        XmlRpcStruct Lookup(XmlRpcStruct rpcStruct);
    }

    public class ProductService : IProductService
    {
        private readonly IProductsRepository repository;
        private readonly ISettingsService appSettingsService;
        IUPCDatabase _proxy;
        public ProductService(IProductsRepository aRepository)
        {
            repository = aRepository;
            appSettingsService = new SettingsService(new SettingsRepository(new BoozicEntities()));

        }

        public Product GetById(int aProductID)
        {
            return repository.GetById(aProductID);
        }
        public Models.ProductInfo GetByUPC(string aUPC)
        {
            return repository.GetByUPC(aUPC);

        }


        public Models.ProductInfo getProductUsingAPI(string UPC)
        {


            String UPCAPIKey = appSettingsService.GetUPCAPIKey();
            //HttpClient httpUPCClient = new HttpClient();
            //HttpResponseMessage UPC_Response = httpUPCClient.GetAsync("http://api.upcdatabase.org/json/" + UPCAPIKey + "/" + UPC).Result;
            //if (UPC_Response.IsSuccessStatusCode)
            //{

            //    string upcResult = UPC_Response.Content.ReadAsStringAsync().Result.ToString();
            //    // parsing Data out of JSON result from google places API
            //    JObject json = Newtonsoft.Json.Linq.JObject.Parse(upcResult);
            //    string productName = json["itemname"].ToString();
            //    if (productName == string.Empty)
            //    {
            //         productName = json["description"].ToString();
            //    }
            //    return productName;
            //}


            // Validate UPC
            if (UPC.Length > 12 && UPC.StartsWith("0"))
                UPC = UPC.Remove(0, 1);
            if (UPC.Length > 12 && UPC.EndsWith("0"))
                UPC = UPC.Remove(UPC.Length - 1, 1);
            // end Validate UPC

            Models.ProductInfo p = new Models.ProductInfo();

            _proxy = XmlRpcProxyGen.Create<IUPCDatabase>();
            XmlRpcStruct request = new XmlRpcStruct();
            XmlRpcStruct response = new XmlRpcStruct();

            request.Add("rpc_key", UPCAPIKey);
            request.Add("upc", UPC);

            response = _proxy.Lookup(request);

            p.UPC = UPC;
            if (response["status"].ToString() != "fail")
            {
                p.ProductName = response["description"].ToString();
                p.SizeInfo = response["size"].ToString();
            }
            p.IsFoundInDatabase = false;
            return p;
        }
    }
}