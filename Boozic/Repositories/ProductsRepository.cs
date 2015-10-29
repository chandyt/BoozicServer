using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

        public List<vwProductsWithStorePrice> filterProducts(decimal latitude, decimal longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 0, int LowestPrice = 0,
                                int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                bool SortByProductType = false, bool SortByDistance = false, bool SortByPrice = true, bool SortByRating = false,
                                bool SortAscending = true)
            {
                List<vwProductsWithStorePrice> Sales = sdContext.vwProductsWithStorePrices.ToList();
                if (ProductTypeId > 0)
                {
                    Sales = Sales.FindAll(s => s.ProductId.Equals(ProductTypeId));
                }
                if (ProductParentTypeId > 0)
                {
                    Sales = Sales.FindAll(s => s.ProductParentTypeId.Equals(ProductParentTypeId));
                }
                if (LowestPrice >=0)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.Price >= LowestPrice; });
                }

                if (HighestPrice < 9999999)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.Price <= HighestPrice; });
                }

                if (LowestRating >=0)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.CombinedRating >= LowestRating; });
                }
                if (HighestRating <=5)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.CombinedRating <= HighestRating; });
                }

                if (LowestABV >= 0)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.ABV >= LowestABV; });
                }
                if (LowestABV <= 5)
                {
                    Sales = Sales.FindAll(delegate(vwProductsWithStorePrice s) { return s.ABV <= HighestABV; });
                }

                if (SortByProductType )
                 {
                     Sales = Sales.OrderBy(o => o.ProductType).ToList();
                 }

                if (SortByProductType)
                {
                    if (SortAscending)
                        Sales = Sales.OrderBy(o => o.ProductType).ToList();
                    else
                        Sales = Sales.OrderByDescending(o => o.ProductType).ToList();
                }

                if (SortByDistance)
                {
                    //if (SortAscending)
                        //Sales = Sales.OrderBy(o => o.ProductType).ToList();
                    //else
                       // Sales = Sales.OrderByDescending(o => o.ProductType).ToList();
                }

                if (SortByPrice)
                {
                    if (SortAscending)
                        Sales = Sales.OrderBy(o => o.Price).ToList();
                    else
                        Sales = Sales.OrderByDescending(o => o.Price).ToList();
                }

                if (SortByRating)
                {
                    if (SortAscending)
                        Sales = Sales.OrderBy(o => o.CombinedRating).ToList();
                    else
                        Sales = Sales.OrderByDescending(o => o.CombinedRating).ToList();
                }

              
                //TODO: Calculate for radius
                return Sales;
            }




    }
}