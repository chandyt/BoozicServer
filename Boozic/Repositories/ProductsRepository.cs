using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Boozic.Services;
using System.ComponentModel;

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

        public List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                 int SortOption = 0)
            {

                List<vwProductsWithStorePrice> lstProducts = sdContext.vwProductsWithStorePrices.ToList();
                lstProducts = lstProducts.OrderBy(o => o.StoreID).ToList();
                List<Models.ProductInfo> lstProductInfo = new List<Models.ProductInfo>();

                //calculating distance and filtering by radius
                int currentStoreId = 0;
                double DistanceFromCurrentLocation = 0;
                LocationService ls = new LocationService();
                StoreService ss = new StoreService(new StoreRepository(new BoozicEntities()));

                foreach (vwProductsWithStorePrice p in lstProducts)
                {
                    Models.ProductInfo tmpProductInfo = new Models.ProductInfo();
                    CopyProperties(tmpProductInfo, p);
                    if (p.StoreID != currentStoreId)
                    {
                        DistanceFromCurrentLocation=ls.getDistanceAndTime(latitude, longitude, (double)p.Latitude, (double)p.Longitude)["Distance"];
                        currentStoreId = p.StoreID;
                    }
                    
                    lstProductInfo.Add(tmpProductInfo);
                    tmpProductInfo.DistanceCalculated = (decimal)DistanceFromCurrentLocation;
                }

                lstProductInfo = lstProductInfo.OrderBy(o => o.ProductId).ToList();
                int currentProductID = 0;
                for (int i = 0; i < lstProductInfo.Count; i++)
                {
                    if (lstProductInfo[i].ProductId != currentProductID)
                    {
                        Models.StoreInfo storeInfo = ls.getStores((double)lstProductInfo[i].Latitude, (double)lstProductInfo[i].Longitude, 0.2)[0];
                        //Store storeInfo = ss.GetById(lstProductInfo[i].StoreID);
                        lstProductInfo.Where(x => x.ProductId == lstProductInfo[i].ProductId).ToList().ForEach(f => f.ClosestStore = storeInfo);
                        //Models.StoreInfo CheapestStore = ls.getStores((double)lstProductInfo[i].Latitude, (double)lstProductInfo[i].Longitude, 0.2)[0];
                        //Store CheapestStore = ss.GetById(lstProductInfo[i].StoreID);
                        lstProductInfo.Where(x => x.ProductId == lstProductInfo[i].ProductId).ToList().ForEach(f => f.CheapestStore = storeInfo);
                        
                        currentProductID = lstProductInfo[i].ProductId;
                    }
                    else
                    {
                       // Store storeInfo = ss.GetById(lstProductInfo[i].StoreID);
                        Models.StoreInfo storeInfo = ls.getStores((double)lstProductInfo[i].Latitude, (double)lstProductInfo[i].Longitude, 0.2)[0];
                        if (lstProductInfo[i].DistanceCalculated < lstProductInfo[i - 1].DistanceCalculated)
                        {
                            // Models.StoreInfo ClosestStore = ls.getStores((double)lstProductInfo[i].Latitude, (double)lstProductInfo[i].Longitude, 0.2)[0];

                            lstProductInfo.Where(x => x.ProductId == lstProductInfo[i].ProductId).ToList().ForEach(f => f.ClosestStore = storeInfo);
                           
                        }

                        if (lstProductInfo[i].Price < lstProductInfo[i - 1].Price)
                        {
                            //Models.StoreInfo CheapestStore = ls.getStores((double)lstProductInfo[i].Latitude, (double)lstProductInfo[i].Longitude, 0.2)[0];
                            //Store CheapestStore = ss.GetById(lstProductInfo[i].StoreID);
                            lstProductInfo.Where(x => x.ProductId == lstProductInfo[i].ProductId).ToList().ForEach(f => f.CheapestStore = storeInfo);
                        }
                    }

                }


                lstProductInfo = lstProductInfo.GroupBy(x => x.ProductId).Select(g => g.First()).ToList();
                //Default Radius =2 Miles
                lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.DistanceCalculated <= Radius; });

               

                if (ProductTypeId > 0)
                {
                    lstProductInfo = lstProductInfo.FindAll(s => s.ProductId.Equals(ProductTypeId));
                }

                    

                if (ProductParentTypeId > 0)
                {
                    String prodFilter = Convert.ToString(ProductParentTypeId, 2).PadLeft(3,'0');
                    int Wine = 1;
                    int Beer = 2;
                    int Liquors = 3;

                    if (prodFilter.Substring(0, 1) == "0")
                        Wine = 0;
                    if (prodFilter.Substring(1, 1) == "0")
                        Beer = 0;
                    if (prodFilter.Substring(2, 1) == "0")
                        Liquors = 0;
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.ProductParentTypeId == Wine || s.ProductParentTypeId == Beer || s.ProductParentTypeId == Liquors; });
                    
                }
                if (LowestPrice >= 0)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.Price >= LowestPrice; });
                }

                if (HighestPrice <= 9999999)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.Price <= HighestPrice; });
                }

                if (LowestRating >= 0)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.CombinedRating >= LowestRating; });
                }
                if (HighestRating <= 5)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.CombinedRating <= HighestRating; });
                }

                if (LowestABV >= 0)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.ABV >= LowestABV; });
                }
                if (LowestABV <= 5)
                {
                    lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.ABV <= HighestABV; });
                }

                if (SortOption > 0)
                {
                    String prodSort = Convert.ToString(SortOption, 2).PadLeft(6, '0');

                    if (prodSort.Substring(0, 1) == "1")
                            lstProductInfo = lstProductInfo.OrderBy(o => o.Price).ToList();
                    if (prodSort.Substring(1, 1) == "1")
                            lstProductInfo = lstProductInfo.OrderByDescending(o => o.Price).ToList();
                    
                    if (prodSort.Substring(2, 1) == "1")
                        lstProductInfo = lstProductInfo.OrderBy(o => o.ABV).ToList();
                    if (prodSort.Substring(3, 1) == "1")
                        lstProductInfo = lstProductInfo.OrderByDescending(o => o.ABV).ToList();
                    
                    if (prodSort.Substring(4, 1) == "1")
                        lstProductInfo = lstProductInfo.OrderBy(o => o.CombinedRating).ToList();
                    if (prodSort.Substring(5, 1) == "1")
                        lstProductInfo = lstProductInfo.OrderByDescending(o => o.CombinedRating).ToList();
                    
                }
                return lstProductInfo;
            }


        static void CopyProperties(object dest, object src)
        {
            foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
            {
                item.SetValue(dest, item.GetValue(src));
            }
        }

    }
}