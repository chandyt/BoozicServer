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
using Boozic.Services;
using Boozic.Repositories;

namespace Boozic.Controllers
{
    public class LocationController : ApiController
    {
        private readonly ILocationService locationService;
        /// <summary>
        /// API function for getting the Liquor store where the User is
        /// </summary>
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <returns>XML data about the store</returns>

        public LocationController()
        {
            locationService = new LocationService();
            
        }

        [HttpGet]
        public IHttpActionResult getStoreInfo(double Latitude, double Longitude)
        {
            StoreInfo SI=new StoreInfo();
            List<StoreInfo> lstSI = new List<StoreInfo>();
            lstSI = locationService.getStores(Latitude, Longitude, 0.2); // Radius is ~150 feet. to accomadate if the user in parking lots...
            //TODO:Insert into stores table
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
            lstSI = locationService.getStores(Latitude, Longitude, Radius);
            //TODO: Insert into stores table
            return Ok(lstSI);
        }


    }
}