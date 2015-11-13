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
        public Models.ProductInfo GetByUPC(string aUPC, double latitude, double longitude)
        {
            return repository.GetByUPC(aUPC, latitude, longitude);

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





            _proxy = XmlRpcProxyGen.Create<IUPCDatabase>();
            XmlRpcStruct request = new XmlRpcStruct();
            XmlRpcStruct response = new XmlRpcStruct();

            Models.ProductInfo pi = new Models.ProductInfo();

            request.Add("rpc_key", UPCAPIKey);
            request.Add("upc", UPC);

            response = _proxy.Lookup(request);


            if (response["status"].ToString() != "fail")
            {
                string size = response["size"].ToString();

                pi.ProductName = response["description"].ToString();

                if (size.Trim() != "")
                {
                    pi.Volume = (double)Convert.ToDecimal(Regex.Replace(size, @"[^0-9\.]", string.Empty));
                    pi.VolumeUnit = size.Replace(pi.Volume.ToString(), string.Empty).Trim();
                }
                else
                {
                    pi.Volume = -1;
                    pi.VolumeUnit = "N/A";
                }

                pi.UPC = UPC;
                pi.ProductTypeId = 4;
                pi.IsFoundInDatabase = 1;

                Product pr = new Product();
                pr.Name = pi.ProductName;
                pr.UPC = UPC;
                pr.Volume = (decimal)pi.Volume;
                pr.VolumeUnit = pi.VolumeUnit;
                pr.TypeDetailsId = 4;
                pi.ProductId = repository.addProduct(pr);

            }
            else
            {
                pi.IsFoundInDatabase = 2;
            }

            return pi;
        }

        public void addProduct(Product aProduct)
        {
            repository.addProduct(aProduct);
        }

        public List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, double LowestPrice = 0,
                                      double HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, double LowestABV = 0, double HighestABV = 100,
                                       int SortOption = 0, bool SortByCheapestStorePrice = false, string DeviceId = "-1")
        {
            return repository.filterProducts(latitude, longitude, ProductTypeId, ProductParentTypeId, Radius, LowestPrice, HighestPrice,
                           LowestRating, HighestRating, LowestABV, HighestABV, SortOption, SortByCheapestStorePrice, DeviceId);
        }

        public String UpdateProduct(int ProductId, int StoreId = -1, double Price = -1, string ProductName = "-1", int ProductTypeId = -1, double ABV = -1, double Volume = -1, string VolumeUnit = "-1", string ContainerType = "-1", string DeviceId = "-1", int Rating = -1)
        {
            return repository.UpdateProduct(ProductId, StoreId, Price, ProductName, ProductTypeId, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
        }

        //public String InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0)
        //{
        //    return repository.InsertProduct(UPC, ProductName, ProductTypeID, StoreId, Price, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
        //}

        public Dictionary<int, string> getParentTypes()
        {

           return repository.getParentTypes();
         }


        public Dictionary<int, string> getProductTypes(int ParentId)
         {
             return repository.getProductTypes(ParentId);
         }
    }
}