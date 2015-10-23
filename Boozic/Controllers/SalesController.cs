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
    public class SalesController : ApiController
    {
        private readonly ISalesService salesService;
        public SalesController()
        {

            salesService = new SalesService(new SalesRepository(new BoozicEntities()));
            
        }
        // GET api/sales
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/sales/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sales
        public void Post([FromBody]string value)
        {
        }

        // PUT api/sales/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sales/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult getSales(int ProductTypeId =0, int ProductParentTypeId=0, int Radius=0, int LowestPrice=0, int HighestPrice=9999999)
        {
            //Add rating and ABV filter
           List<vwSale> Sales= salesService.getSales(ProductTypeId ,  ProductParentTypeId,  Radius,  LowestPrice,  HighestPrice);
           return Ok(Sales);
        }
    }
}
