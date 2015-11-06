using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;
using System.Collections.Specialized;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using CookComputing.XmlRpc;
using System.Text.RegularExpressions;

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
            //if (UPC.Length > 12 && UPC.EndsWith("0"))
            //    UPC = UPC.Remove(UPC.Length - 1, 1);
            // end Validate UPC

            

            _proxy = XmlRpcProxyGen.Create<IUPCDatabase>();
            XmlRpcStruct request = new XmlRpcStruct();
            XmlRpcStruct response = new XmlRpcStruct();
           
            Models.ProductInfo pi=new Models.ProductInfo();

            request.Add("rpc_key", UPCAPIKey);
            request.Add("upc", UPC);

            response = _proxy.Lookup(request);


            if (response["status"].ToString() != "fail")
            {
                string size = response["size"].ToString();

                pi.ProductName = response["description"].ToString();
                pi.Volume =(double) Convert.ToDecimal(Regex.Replace(size, @"[^0-9\.]", string.Empty));
                pi.VolumeUnit = size.Replace(pi.Volume.ToString(), string.Empty).Trim();
                pi.UPC = UPC;
                pi.ProductTypeId = 4;
                pi.IsFoundInDatabase = false;
            }

           
            return pi;
        }

       public void addProduct(Product aProduct)
        {
            repository.addProduct(aProduct);
        }

       public List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                     int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                      int SortOption = 0, bool SortByCheapestStorePrice = false)
       {
           return repository.filterProducts(latitude, longitude, ProductTypeId, ProductParentTypeId, Radius, LowestPrice, HighestPrice,
                          LowestRating, HighestRating, LowestABV, HighestABV, SortOption, SortByCheapestStorePrice);
       }

       public String UpdateProduct(int ProductId, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId, int Rating)
       {
           return repository.UpdateProduct(ProductId, StoreId, Price, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
       }

       public String InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0)
       {
           return repository.InsertProduct(UPC, ProductName,  ProductTypeID, StoreId, Price, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
       }
    }
}