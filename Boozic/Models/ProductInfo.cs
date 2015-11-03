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
    public class ProductInfo : vwProductsWithStorePrice
    {

        public ProductInfo()
        { }

        //int productId;
        //string productName;
        //int productTypeId;
        //string productType;
        //double abv;
        //string upc;
        //string volumeUnit;
        //double volume;
        bool isFoundInDatabase=true;
        decimal distanceCalcualted=0;
        StoreInfo cheapestStore;
        StoreInfo closestStore;
        
        //[JsonProperty(PropertyName = "ProductID")]
        //public int ProductId
        //{
        //    get { return productId; }
        //    set { productId = value; }
        //}

        //[JsonProperty(PropertyName = "UPC")]
        //public string UPC
        //{
        //    get { return upc; }
        //    set { upc = value; }
        //}

        //[JsonProperty(PropertyName = "ProductName")]
        //public string ProductName
        //{
        //    get { return productName; }
        //    set { productName = value; }
        //}

        //[JsonProperty(PropertyName = "ProductTypeId")]
        //public int ProductTypeId
        //{
        //    get { return productTypeId; }
        //    set { productTypeId = value; }
        //}

        //[JsonProperty(PropertyName = "ProductType")]
        //public string ProductType
        //{
        //    get { return productType; }
        //    set { productType = value; }
        //}

        //[JsonProperty(PropertyName = "ABV")]
        //public double ABV
        //{
        //    get { return abv; }
        //    set { abv = value; }
        //}


        //[JsonProperty(PropertyName = "VolumeUnit")]
        //public string VolumeUnit
        //{
        //    get { return volumeUnit; }
        //    set { volumeUnit = value; }
        //}

        //[JsonProperty(PropertyName = "Volume")]
        //public double Volume
        //{
        //    get { return volume; }
        //    set { volume = value; }
        //}

        [JsonProperty(PropertyName = "IsFoundInDatabase")]
        public bool IsFoundInDatabase
        {
            get { return isFoundInDatabase; }
            set { isFoundInDatabase = value; }
        }

        [JsonProperty(PropertyName = "DistanceCalculatedInMiles")]
        public decimal DistanceCalculated
        {
            get { return distanceCalcualted; }
            set { distanceCalcualted = value; }
        }

        [JsonProperty(PropertyName = "CheapestStore")]
        public StoreInfo CheapestStore
        {
            get { return cheapestStore; }
            set { cheapestStore = value; }
        }

        [JsonProperty(PropertyName = "ClosestStore")]
        public StoreInfo ClosestStore
        {
            get { return closestStore; }
            set { closestStore = value; }
        }

    }


}