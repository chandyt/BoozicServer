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
    public class Product
    {

        int productId;
        string productName;
        int productTypeId;
        string productType;
        double abv;
        string upc;
        
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

        [JsonProperty(PropertyName = "ABV")]
        public double ABV
        {
            get { return abv; }
            set { abv = value; }
        }



    }


}