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
    public class ProductInfo
    {

        public ProductInfo()
        { }

        int productId = -1;
        string productName;
        int productTypeId = -1;
        string productType;
        double abv = -1;
        string upc;
        string volumeUnit;
        double volume = -1;
        int isFoundInDatabase = 0;
        StorePrice cheapestStore;
        StorePrice closestStore;
        int rating1, rating2, rating3, rating4, rating5;
        double combinedRating = -1;
        int ratingByCurrentUser = -1;
        int productParentTypeId = -1;
        string productParentType;
        string containerType;
        int containerQty;
        bool isClosestStoreAndCheapestStoreSame;
        int isFavourite = 0;

        [JsonProperty(PropertyName = "ProductID")]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [JsonProperty(PropertyName = "UPC")]
        public string UPC
        {
            get { return upc; }
            set { upc = value; }
        }

        [JsonProperty(PropertyName = "ProductName")]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [JsonProperty(PropertyName = "ProductTypeId")]
        public int ProductTypeId
        {
            get { return productTypeId; }
            set { productTypeId = value; }
        }

        [JsonProperty(PropertyName = "ProductType")]
        public string ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        [JsonProperty(PropertyName = "ProductParentTypeId")]
        public int ProductParentTypeId
        {
            get { return productParentTypeId; }
            set { productParentTypeId = value; }
        }

        [JsonProperty(PropertyName = "ProductParentType")]
        public string ProductParentType
        {
            get { return productParentType; }
            set { productParentType = value; }
        }

        [JsonProperty(PropertyName = "ABV")]
        public double ABV
        {
            get { return abv; }
            set { abv = value; }
        }


        [JsonProperty(PropertyName = "VolumeUnit")]
        public string VolumeUnit
        {
            get { return volumeUnit; }
            set { volumeUnit = value; }
        }

        [JsonProperty(PropertyName = "Volume")]
        public double Volume
        {
            get
            {
                return volume;
            }
            set { volume = value; }
        }

        [JsonProperty(PropertyName = "ContainerType")]
        public string ContainerType
        {
            get { return containerType; }
            set { containerType = value; }
        }

        [JsonProperty(PropertyName = "ContainerQty")]
        public int ContainerQty
        {
            get { return containerQty; }
            set { containerQty = value; }
        }


        [JsonProperty(PropertyName = "IsFoundInDatabase")]
        public int IsFoundInDatabase
        {
            get { return isFoundInDatabase; }
            set { isFoundInDatabase = value; }
        }

        [JsonProperty(PropertyName = "CheapestStore")]
        public StorePrice CheapestStore
        {
            get { return cheapestStore; }
            set { cheapestStore = value; }
        }

        [JsonProperty(PropertyName = "ClosestStore")]
        public StorePrice ClosestStore
        {
            get { return closestStore; }
            set { closestStore = value; }
        }

        [JsonProperty(PropertyName = "Rating1")]
        public int Rating1
        {
            get { return rating1; }
            set { rating1 = value; }
        }

        [JsonProperty(PropertyName = "Rating2")]
        public int Rating2
        {
            get { return rating2; }
            set { rating2 = value; }
        }

        [JsonProperty(PropertyName = "Rating3")]
        public int Rating3
        {
            get { return rating3; }
            set { rating3 = value; }
        }

        [JsonProperty(PropertyName = "Rating4")]
        public int Rating4
        {
            get { return rating4; }
            set { rating4 = value; }
        }

        [JsonProperty(PropertyName = "Rating5")]
        public int Rating5
        {
            get { return rating5; }
            set { rating5 = value; }
        }

        [JsonProperty(PropertyName = "CombinedRating")]
        public double CombinedRating
        {
            get { return combinedRating; }
            set { combinedRating = value; }
        }

        [JsonProperty(PropertyName = "RatingByCurrentUser")]
        public int RatingByCurrentUser
        {
            get { return ratingByCurrentUser; }
            set { ratingByCurrentUser = value; }
        }


        [JsonProperty(PropertyName = "IsClosestStoreAndCheapestStoreSame")]
        public Boolean IsClosestStoreAndCheapestStoreSame
        {
            get { return isClosestStoreAndCheapestStoreSame; }
            set { isClosestStoreAndCheapestStoreSame = value; }
        }

        [JsonProperty(PropertyName = "IsFavourite")]
        public int IsFavourite
        {
            get { return isFavourite; }
            set { isFavourite = value; }
        }


    }


}