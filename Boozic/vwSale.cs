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
    
    public partial class vwSale
    {
        public int SaleId { get; set; }
        public int StoreID { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> ProductParentTypeId { get; set; }
        public string UPC { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> ABV { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Type { get; set; }
        public string ProductType { get; set; }
        public string StoreName { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    }
}
