//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Boozic
{
    using System;
    using System.Collections.Generic;
    
    public partial class vwProductsWithStorePrice
    {
        public int StoreID { get; set; }
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public Nullable<int> ProductParentTypeId { get; set; }
        public string UPC { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> ABV { get; set; }
        public string Type { get; set; }
        public string ProductType { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public string VolumeUnit { get; set; }
        public string ContainerType { get; set; }
        public string StoreName { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<int> Rating1 { get; set; }
        public Nullable<int> Rating2 { get; set; }
        public Nullable<int> Rating3 { get; set; }
        public Nullable<int> Rating4 { get; set; }
        public Nullable<int> Rating5 { get; set; }
        public string ProductName { get; set; }
        public Nullable<decimal> CombinedRating { get; set; }
        public int PriceID { get; set; }
    }
}
