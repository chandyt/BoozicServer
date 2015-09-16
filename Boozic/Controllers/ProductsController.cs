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



        public IHttpActionResult getProductInfo(string UPC)
        {
            Models.ProductInfo p = null;
            if (UPC!= string.Empty)
            {
                //0085976033931
                //5410316442930
              p=  productService.GetByUPC(UPC);
              if (p.UPC == null)
              {
                  p.ProductName = productService.getProductName(UPC);
                    
              }
            }

            return Ok(p);
        }
    }
}
