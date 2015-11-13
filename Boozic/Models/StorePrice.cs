using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace Boozic.Models
{
    public class StorePrice 
    {
        public StorePrice()
        {
        }
        public StorePrice(StoreInfo aStoreInfo)
        {
            this.Distance = aStoreInfo.Distance;
            //this.Duration = aStoreInfo.Duration;
            //this.IsOpenNow = aStoreInfo.IsOpenNow;
            this.Latitude = aStoreInfo.Latitude;
            this.Longitude = aStoreInfo.Longitude;
            this.StoreAddress = aStoreInfo.StoreAddress;
            this.StoreID = aStoreInfo.StoreID;
            this.StoreName = aStoreInfo.StoreName;
        }

        double price = 0;
        string lastUpdated;
        int storeID;
        string storeName;
        string storeAddress;
        double latitude;
        double longitude;
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
        /// Distance in Miles
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "DistanceInMiles")]
        public double Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        ///// <summary>
        ///// Duration in Seconds
        ///// </summary>
        //[JsonProperty(PropertyName = "DurationInSeconds")]
        //public double Duration
        //{
        //    get { return duration; }
        //    set { duration = value; }
        //}

        //[JsonProperty(PropertyName = "IsOpenNow")]
        //public bool IsOpenNow
        //{
        //    get { return isOpenNow; }
        //    set { isOpenNow = value; }
        //}
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string LastUpdated
        {
            get { return lastUpdated; }
            set { lastUpdated = value; }
        }
    }
}