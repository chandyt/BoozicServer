using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Boozic.Controllers
{
    public class SalesController : ApiController
    {
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

        public IHttpActionResult getSales(int ProductType =0, int ProductParentType=0, int Radius=0, int LowestPrice=0, int HighestPrice=9999999)
        {
            //TODO: Get the sales data and do filters based on parameters
            return Ok(Radius);
        }
    }
}
