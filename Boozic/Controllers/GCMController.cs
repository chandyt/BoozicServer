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
    public class GCMController : ApiController
    {
        private readonly IGCMService gcmService;
        /// <summary>
        /// API function for getting the Liquor store where the User is
        /// </summary>
        /// <param name="Latitude">Current Latitude</param>
        /// <param name="Longitude">Current Longitude</param>
        /// <returns>XML data about the store</returns>

        public GCMController()
        {
            gcmService = new GCMService(new GCMRepository(new BoozicEntities()));
            
        }
        [HttpGet]
        public IHttpActionResult addGCMRegkey(string RegKey, string DeviceId)
        {
            if (gcmService.GetByDeviceID(DeviceId)==null)
            {
                GCMRegKey aKey=new GCMRegKey();
                aKey.DeviceId = DeviceId;
                aKey.RegistrationToken = RegKey;
                aKey.RegisterdOn = DateTime.Now;
                gcmService.Add(aKey);
                
            }
            return Ok("Key Added");
        }


        [HttpGet]
        public IHttpActionResult getAllKey()
        {
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult SendMessage(string Message)
        {
            gcmService.SendNotification(Message);
            return Ok();
        }
        
    }
}
