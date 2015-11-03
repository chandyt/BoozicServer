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
    
    public partial class Product
    {
        public Product()
        {
            this.ProductsPrices = new HashSet<ProductsPrice>();
            this.UserProductRatings = new HashSet<UserProductRating>();
            this.UserFavourites = new HashSet<UserFavourite>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string UPC { get; set; }
        public Nullable<decimal> ABV { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public string VolumeUnit { get; set; }
        public string ContainerType { get; set; }
        public int TypeDetailsId { get; set; }
    
        public virtual ProductRating ProductRating { get; set; }
        public virtual ICollection<ProductsPrice> ProductsPrices { get; set; }
        public virtual ICollection<UserProductRating> UserProductRatings { get; set; }
        public virtual TypesDetail TypesDetail1 { get; set; }
        public virtual ICollection<UserFavourite> UserFavourites { get; set; }
    }
}
