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
        public IHttpActionResult getProductInfo(string UPC)
        {
            Models.ProductInfo p = null;
            if (UPC!= string.Empty)
            {
                //0085976033931
                //5410316442930
              p=  productService.GetByUPC(UPC);
              if (p.IsFoundInDatabase==false)
              {
                  p = productService.getProductUsingAPI(UPC);
                    
              }
            }

            return Ok(p);
        }

        [HttpGet]
        public IHttpActionResult getProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                    int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV=0, int HighestABV=100,
                                    int SortOption = 0, bool SortByCheapestStorePrice = false)
        {

            List<Models.ProductInfo> products = productService.filterProducts(latitude, longitude, ProductTypeId, ProductParentTypeId, Radius, LowestPrice, HighestPrice,
                                      LowestRating, HighestRating, LowestABV, HighestABV, SortOption, SortByCheapestStorePrice);
            return Ok(products);
        }


        [HttpGet]
        public IHttpActionResult updateProduct(int ProductId, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId, int Rating)
        {

            string UpdateStatus =  productService.UpdateProduct( ProductId,  StoreId,  Price,  ABV,  Volume,  VolumeUnit,  ContainerType,  DeviceId,  Rating);
            return Ok(UpdateStatus);
        }

        [HttpGet]
        public IHttpActionResult InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0)
        {
            string InsertStatus = productService.InsertProduct(UPC,ProductName,   ProductTypeID,StoreId, Price, ABV, Volume, VolumeUnit, ContainerType, DeviceId, Rating);
            return Ok(InsertStatus);
        }
    }
}
