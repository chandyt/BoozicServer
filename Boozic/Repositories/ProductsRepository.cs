using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Boozic.Services;

namespace Boozic.Repositories
{
    public class ProductsRepository : BaseContentRepository<Product>, IProductsRepository
    {
        public BoozicEntities sdContext { get; private set; }
        public ProductsRepository(BoozicEntities ctx)
            : base(ctx)
        {
            sdContext = ctx;

        }

        public IEnumerable<Product> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<Product> GetObjectSet()
        {
            return sdContext.Products;
        }

        public override Product GetById(object id)
        {
            if (id is int)
            {
                return sdContext.Products.SingleOrDefault(x => x.Id == (int)id);
            }
            return null;
        }
        public Models.ProductInfo GetByUPC(string UPC)
        {
            if (UPC != string.Empty)
            {
                Models.ProductInfo pr = (Models.ProductInfo)sdContext.vwProductsWithStorePrices.SingleOrDefault(x => x.UPC == (string)UPC);
                if (pr.UPC != null)
                {
                    pr.IsFoundInDatabase = true;

                }
                return pr;
            }
            return null;
        }

        public void addProduct(Product aProduct)
        {
            sdContext.Products.Add(aProduct);
            sdContext.SaveChanges();

        }

        public List<vwProductsWithStorePrice> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                bool SortByProductType = false, bool SortByDistance = false, bool SortByPrice = true, bool SortByRating = false,
                                bool SortAscending = true)
            {

                List<vwProductsWithStorePrice> lstProducts = sdContext.vwProductsWithStorePrices.ToList();
                lstProducts = lstProducts.OrderBy(o => o.StoreID).ToList();
             
                //calculating distance and filtering by radius
                int currentStoreId = 0;
                double DistanceFromCurrentLocation = 0;
                LocationService lc = new LocationService();
                foreach (vwProductsWithStorePrice p in lstProducts)
                {
                    p.DistanceFromCurrentLocation = 0;
                    if (p.StoreID != currentStoreId)
                    {
                        DistanceFromCurrentLocation=lc.getDistanceAndTime(latitude, longitude, (double)p.Latitude, (double)p.Longitude)["Distance"];
                        currentStoreId = p.StoreID;
                    }
                    p.DistanceFromCurrentLocation =(decimal) DistanceFromCurrentLocation;
                }
                
                //Default Radius =2 Miles
                lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.DistanceFromCurrentLocation <= Radius; });

                if (ProductTypeId > 0)
                {
                    lstProducts = lstProducts.FindAll(s => s.ProductId.Equals(ProductTypeId));
                }
                if (ProductParentTypeId > 0)
                {
                    lstProducts = lstProducts.FindAll(s => s.ProductParentTypeId.Equals(ProductParentTypeId));
                }
                if (LowestPrice >= 0)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.Price >= LowestPrice; });
                }

                if (HighestPrice <= 9999999)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.Price <= HighestPrice; });
                }

                if (LowestRating >= 0)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.CombinedRating >= LowestRating; });
                }
                if (HighestRating <= 5)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.CombinedRating <= HighestRating; });
                }

                if (LowestABV >= 0)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.ABV >= LowestABV; });
                }
                if (LowestABV <= 5)
                {
                    lstProducts = lstProducts.FindAll(delegate(vwProductsWithStorePrice s) { return s.ABV <= HighestABV; });
                }

                if (SortByProductType)
                {
                    lstProducts = lstProducts.OrderBy(o => o.ProductType).ToList();
                }

                if (SortByProductType)
                {
                    if (SortAscending)
                        lstProducts = lstProducts.OrderBy(o => o.ProductType).ToList();
                    else
                        lstProducts = lstProducts.OrderByDescending(o => o.ProductType).ToList();
                }

                if (SortByDistance)
                {
                    if (SortAscending)
                        lstProducts = lstProducts.OrderBy(o => o.ProductType).ToList();
                    else
                        lstProducts = lstProducts.OrderByDescending(o => o.ProductType).ToList();
                }

                if (SortByPrice)
                {
                    if (SortAscending)
                        lstProducts = lstProducts.OrderBy(o => o.Price).ToList();
                    else
                        lstProducts = lstProducts.OrderByDescending(o => o.Price).ToList();
                }

                if (SortByRating)
                {
                    if (SortAscending)
                        lstProducts = lstProducts.OrderBy(o => o.CombinedRating).ToList();
                    else
                        lstProducts = lstProducts.OrderByDescending(o => o.CombinedRating).ToList();
                }

                if (SortByDistance)
                {
                    if (SortAscending)
                        lstProducts = lstProducts.OrderBy(o => o.DistanceFromCurrentLocation).ToList();
                    else
                        lstProducts = lstProducts.OrderByDescending(o => o.DistanceFromCurrentLocation).ToList();
                }

                return lstProducts;
            }


        

    }
}