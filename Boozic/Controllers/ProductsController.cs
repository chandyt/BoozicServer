using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Boozic.Services;
using Boozic.Repositories;

namespace Boozic.Controllers
{
    public class ProductsController : ApiController
    {
         private readonly IProductService productService;
        /// <summary>
        /// API function for getting the Liquor store where the User is
        /// </summary>
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <returns>XML data about the store</returns>

         public ProductsController()
        {
            
            productService = new ProductService(new ProductsRepository(new BoozicEntities()));
            
        }
        // GET api/products
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/products/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/products
        public void Post([FromBody]string value)
        {
        }

        // PUT api/products/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/products/5
        public void Delete(int id)
        {
        }


        [HttpGet]
        public IHttpActionResult getProductInfo(string UPC, double latitude, double longitude)
        {
            Models.ProductInfo p = new Models.ProductInfo();
            if (UPC!= string.Empty)
            {
                // Validate UPC
                if (UPC.Length > 12 && UPC.StartsWith("0"))
                    UPC = UPC.Remove(0, 1);
                //if (UPC.Length > 12 && UPC.EndsWith("0"))
                //    UPC = UPC.Remove(UPC.Length - 1, 1);
                // end Validate UPC

                //0085976033931
                //5410316442930
              p=  productService.GetByUPC(UPC, latitude, longitude);
              
              if (p.IsFoundInDatabase!=0)
              {
                  //TODO:store in database
                  p = productService.getProductUsingAPI(UPC);
              }
            }

            return Ok(p);
        }

        [HttpGet]
        public IHttpActionResult getProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, double LowestPrice = 0,
                                    double HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, double LowestABV=0, double HighestABV=100,
                                    int SortOption = 0, bool SortByCheapestStorePrice = false, string DeviceId="-1")
        {

            List<Models.ProductInfo> products = productService.filterProducts(latitude, longitude, ProductTypeId, ProductParentTypeId, Radius, LowestPrice, HighestPrice,
                                      LowestRating, HighestRating, LowestABV, HighestABV, SortOption, SortByCheapestStorePrice, DeviceId);
            return Ok(products);
        }


        [HttpGet]
        public IHttpActionResult updateProduct(int ProductId, int StoreId = -1, double Price=-1, string ProductName="-1", int ProductTypeId=-1, double ABV=-1, double Volume=-1, string VolumeUnit="-1", string ContainerType="-1", string DeviceId="-1", int Rating=-1)
        {

            string UpdateStatus = productService.UpdateProduct(ProductId, StoreId, Price, ProductName, ProductTypeId, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
            return Ok(UpdateStatus);
        }

        //[HttpGet]
        //public IHttpActionResult InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0)
        //{
        //    string InsertStatus = productService.InsertProduct(UPC,ProductName,   ProductTypeID,StoreId, Price, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
        //    return Ok(InsertStatus);
        //}

        [HttpGet]
        public IHttpActionResult getParentTypes()
        {
            Dictionary<int,string> ty = productService.getParentTypes();

            return Ok(ty);
        }

        [HttpGet]
        public IHttpActionResult getProductTypes(int ParentId)
        {
            Dictionary<int, string> ty = productService.getProductTypes(ParentId);
            return Ok(ty);
        }

        [HttpGet]
        public IHttpActionResult getFavourites(string DeviceId, double latitude, double longitude)
        {
            List<Models.ProductInfo> products= productService.getFavourites(DeviceId, latitude,longitude);
            return Ok(products);
        }

        [HttpGet]
        public IHttpActionResult addToFavourites(string DeviceId, int ProductId)
        {
            string status = productService.addToFavourites(DeviceId, ProductId);
            return Ok(status);
        }
    }
}
