using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boozic.Models
{
    /// <summary>
    /// This is the modal class that stores the stores data and returned to the users
    /// </summary>
    public class StoreInfo
    {

        int storeID;
        string storeName;
        string storeAddress;
        double latitude;
        double longitude;
        bool isOpenNow;
        double duration;
        double distance;

        [JsonProperty(PropertyName = "StoreID")]
        public int StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        [JsonProperty(PropertyName = "StoreName")]
        public string StoreName
        {
            get { return storeName; }
            set { storeName = value; }
        }

        [JsonProperty(PropertyName = "Address")]
        public string StoreAddress
        {
            get { return storeAddress; }
            set { storeAddress = value; }
        }

        [JsonProperty(PropertyName = "Latitude")]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [JsonProperty(PropertyName = "Longitude")]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        /// <summary>
        /// Distance in Meters
        /// </summary>
        [JsonProperty(PropertyName = "DistanceInMeters")]
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        /// <summary>
        /// Duration in Seconds
        /// </summary>
        [JsonProperty(PropertyName = "DurationInSeconds")]
        public double Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        [JsonProperty(PropertyName = "IsOpenNow")]
        public bool IsOpenNow
        {
            get { return isOpenNow; }
            set { isOpenNow = value; }
        }

    }


}