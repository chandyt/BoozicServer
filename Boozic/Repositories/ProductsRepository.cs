﻿using System;
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
        public Models.ProductInfo GetByUPC(string UPC, double latitude, double longitude)
        {
            if (UPC != string.Empty)
            {


                Product tmpPr = (Product)sdContext.Products.SingleOrDefault(x => x.UPC == (string)UPC);
                Models.ProductInfo pr = new Models.ProductInfo();
                if (tmpPr != null)
                {
                    pr.ProductId = (int)tmpPr.Id;
                    pr.ProductName = tmpPr.Name;
                    pr.ProductTypeId = (int)tmpPr.TypeDetailsId;
                    pr.UPC = tmpPr.UPC;
                    pr.Volume = (double)tmpPr.Volume.GetValueOrDefault();
                    if (tmpPr.VolumeUnit != null)
                        pr.VolumeUnit = tmpPr.VolumeUnit;
                    else
                        pr.VolumeUnit = "-1";
                    if (tmpPr.ContainerType != null)
                        pr.ContainerType = tmpPr.ContainerType;
                    else
                        pr.ContainerType = "-1";
                    if (tmpPr.ContainerQty != null)
                        pr.ContainerQty = (int)tmpPr.ContainerQty;
                    else
                        pr.ContainerQty = (int)-1;
                    pr.ABV = (double)tmpPr.ABV.GetValueOrDefault();
                    pr.IsFoundInDatabase = 0;

                    TypesDetail tmpDetails = sdContext.TypesDetails.SingleOrDefault(x => x.Id == (int)tmpPr.TypeDetailsId);
                    pr.ProductType = tmpDetails.Description;

                    Type tmpParentType = sdContext.Types.SingleOrDefault(x => x.Id == (int)tmpDetails.ParentId);
                    pr.ProductParentTypeId = tmpParentType.Id;
                    pr.ProductParentType = tmpParentType.Type1;

                    ProductRating tmpRating = sdContext.ProductRatings.SingleOrDefault(x => x.ProductId == (int)tmpPr.Id);
                    if (tmpRating != null)
                    {
                        pr.Rating1 = (int)tmpRating.Rating1;
                        pr.Rating2 = (int)tmpRating.Rating2;
                        pr.Rating3 = (int)tmpRating.Rating3;
                        pr.Rating4 = (int)tmpRating.Rating4;
                        pr.Rating5 = (int)tmpRating.Rating5;
                        pr.CombinedRating = (double)tmpRating.CombinedRating;
                    }
                    else
                    {
                        pr.Rating1 = 0;
                        pr.Rating2 = 0;
                        pr.Rating3 = 0;
                        pr.Rating4 = 0;
                        pr.Rating5 = 0;
                        pr.CombinedRating = 0;
                    }
                    pr.CheapestStore = getCheapestStore(pr.ProductId, latitude, longitude);
                    pr.ClosestStore = getClosestStore(pr.ProductId, latitude, longitude);
                    pr.IsFoundInDatabase = 0;
                    if (pr.ClosestStore.StoreID > 0 && pr.CheapestStore.StoreID > 0 && pr.ClosestStore.StoreID == pr.CheapestStore.StoreID)
                        pr.IsClosestStoreAndCheapestStoreSame = true;
                    else
                        pr.IsClosestStoreAndCheapestStoreSame = false;
                }
                else
                {
                    pr.IsFoundInDatabase = 1;
                }
                return pr;
            }
            return null;
        }

        public int addProduct(Product aProduct)
        {
            sdContext.Products.Add(aProduct);
            sdContext.SaveChanges();
            return aProduct.Id;

        }

        public List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, double LowestPrice = 0,
                              double HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, double LowestABV = 0, double HighestABV = 100,
                               int SortOption = 0, bool SortByCheapestStorePrice = false, string DeviceId = "-1")
        {

            List<Product> lstProducts = sdContext.Products.ToList();
            lstProducts = lstProducts.GroupBy(x => x.Id).Select(g => g.First()).ToList();

            List<Models.ProductInfo> lstProductInfo = new List<Models.ProductInfo>();

            //calculating distance and filtering by radius
            //LocationService ls = new LocationService();
            StoreService ss = new StoreService(new StoreRepository(new BoozicEntities()));

            foreach (Product p in lstProducts)
            {
                Models.ProductInfo tmpProductInfo = new Models.ProductInfo();
                tmpProductInfo.ProductId = p.Id;
                tmpProductInfo.ProductName = p.Name;
                tmpProductInfo.ProductTypeId = p.TypeDetailsId;
                tmpProductInfo.UPC = p.UPC;
                tmpProductInfo.Volume = (double)p.Volume.GetValueOrDefault();
                if (p.VolumeUnit != null)
                    tmpProductInfo.VolumeUnit = p.VolumeUnit;
                else
                    tmpProductInfo.VolumeUnit = "-1";
                if (p.ContainerType != null)
                    tmpProductInfo.ContainerType = p.ContainerType;
                else
                    tmpProductInfo.ContainerType = "-1";
                if (p.ContainerQty != null)
                    tmpProductInfo.ContainerQty = (int)p.ContainerQty;
                else
                    tmpProductInfo.ContainerQty = (int)-1;
                tmpProductInfo.ABV = (double)p.ABV.GetValueOrDefault();
                tmpProductInfo.IsFoundInDatabase = 0;

                TypesDetail tmpDetails = sdContext.TypesDetails.SingleOrDefault(x => x.Id == (int)p.TypeDetailsId);
                tmpProductInfo.ProductType = tmpDetails.Description;

                Type tmpParentType = sdContext.Types.SingleOrDefault(x => x.Id == (int)tmpDetails.ParentId);
                tmpProductInfo.ProductParentTypeId = tmpParentType.Id;
                tmpProductInfo.ProductParentType = tmpParentType.Type1;

                ProductRating tmpRating = sdContext.ProductRatings.SingleOrDefault(x => x.ProductId == (int)p.Id);
                if (tmpRating != null)
                {
                    tmpProductInfo.Rating1 = (int)tmpRating.Rating1;
                    tmpProductInfo.Rating2 = (int)tmpRating.Rating2;
                    tmpProductInfo.Rating3 = (int)tmpRating.Rating3;
                    tmpProductInfo.Rating4 = (int)tmpRating.Rating4;
                    tmpProductInfo.Rating5 = (int)tmpRating.Rating5;
                    tmpProductInfo.CombinedRating = (double)tmpRating.CombinedRating;
                }
                else
                {
                    tmpProductInfo.Rating1 = 0;
                    tmpProductInfo.Rating2 = 0;
                    tmpProductInfo.Rating3 = 0;
                    tmpProductInfo.Rating4 = 0;
                    tmpProductInfo.Rating5 = 0;
                    tmpProductInfo.CombinedRating = 0;
                }

                if (DeviceId != "-1")
                {
                    UserProductRating aUserProductRating = sdContext.UserProductRatings.SingleOrDefault(x => x.ProductId == (int)p.Id && x.DeviceId == DeviceId);
                    if (aUserProductRating != null)
                        tmpProductInfo.RatingByCurrentUser = (int)aUserProductRating.Rating;

                    UserFavourite aUserFavourite = sdContext.UserFavourites.SingleOrDefault(x => x.ProductId == (int)p.Id && x.DeviceId == DeviceId);
                    if (aUserFavourite == null)
                        tmpProductInfo.IsFavourite = 0;
                    else
                        tmpProductInfo.IsFavourite = 1;


                }

                tmpProductInfo.CheapestStore = getCheapestStore((int)p.Id, latitude, longitude);
                tmpProductInfo.ClosestStore = getClosestStore((int)p.Id, latitude, longitude);
                if (tmpProductInfo.ClosestStore.StoreID > 0 && tmpProductInfo.CheapestStore.StoreID > 0 && tmpProductInfo.ClosestStore.StoreID == tmpProductInfo.CheapestStore.StoreID)
                    tmpProductInfo.IsClosestStoreAndCheapestStoreSame = true;
                else
                    tmpProductInfo.IsClosestStoreAndCheapestStoreSame = false;
                lstProductInfo.Add(tmpProductInfo);
            }


            //Default Radius =2 Miles
            lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.CheapestStore.Distance <= Radius || s.ClosestStore.Distance <= Radius; });



            if (ProductTypeId > 0)
            {
                lstProductInfo = lstProductInfo.FindAll(s => s.ProductId.Equals(ProductTypeId));
            }



            if (ProductParentTypeId > 0)
            {
                String prodFilter = Convert.ToString(ProductParentTypeId, 2).PadLeft(3, '0');
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
                lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.ClosestStore.Price >= LowestPrice; });
            }

            if (HighestPrice <= 9999999)
            {
                lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.ClosestStore.Price <= HighestPrice; });
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

                if (prodSort.Substring(0, 1) == "1" && SortByCheapestStorePrice == false)
                    lstProductInfo = lstProductInfo.OrderBy(o => o.ClosestStore.Price).ToList();
                if (prodSort.Substring(1, 1) == "1" && SortByCheapestStorePrice == false)
                    lstProductInfo = lstProductInfo.OrderByDescending(o => o.ClosestStore.Price).ToList();

                if (prodSort.Substring(0, 1) == "1" && SortByCheapestStorePrice == true)
                    lstProductInfo = lstProductInfo.OrderBy(o => o.CheapestStore.Price).ToList();
                if (prodSort.Substring(1, 1) == "1" && SortByCheapestStorePrice == true)
                    lstProductInfo = lstProductInfo.OrderByDescending(o => o.CheapestStore.Price).ToList();

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

        private Models.StorePrice getCheapestStore(int ProductId, double SourceLatitude, double SourceLongitude)
        {
            List<Boozic.ProductsPrice> lstTemp = sdContext.ProductsPrices.ToList().FindAll(delegate(Boozic.ProductsPrice s) { return s.ProductId == ProductId; }).ToList();
            StoreService ss = new StoreService(new StoreRepository(new BoozicEntities()));
            LocationService ls = new LocationService();
            lstTemp = lstTemp.OrderBy(o => o.Price).ToList();
            Models.StorePrice st = new Models.StorePrice();
            if (lstTemp.Count >= 1)
            {
                Store cheapestStore = ss.GetById(lstTemp[0].StoreID);
                //st = new Models.StorePrice(ls.getStores((double)tmpStore.Latitude, (double)tmpStore.Longitude, 0.2)[0]);
                st.StoreID = cheapestStore.Id;
                st.StoreName = cheapestStore.StoreName;
                st.StoreAddress = cheapestStore.Address;
                st.Latitude = (double)cheapestStore.Latitude;
                st.Longitude = (double)cheapestStore.Longitude;

                st.Price = (double)lstTemp[0].Price;
                st.LastUpdated = Convert.ToDateTime(lstTemp[0].LastUpdated).ToString("MM/dd/yy");
                st.StoreID = lstTemp[0].StoreID;

                double tmpDistance = ls.distance(SourceLatitude, SourceLongitude, (double)cheapestStore.Latitude, (double)cheapestStore.Longitude);
                st.Distance = tmpDistance;

            }
            return st;
        }

        private Models.StorePrice getClosestStore(int ProductId, double SourceLatitude, double SourceLongitude)
        {
            List<Boozic.ProductsPrice> lstTemp = sdContext.ProductsPrices.ToList().FindAll(delegate(Boozic.ProductsPrice s) { return s.ProductId == ProductId; }).ToList();
            StoreService ss = new StoreService(new StoreRepository(new BoozicEntities()));
            LocationService ls = new LocationService();

            int closestStoreId = 0;
            double DistanceFromCurrentLocation = 9999999;
            double ClosestStorePrice = 0;
            //DateTime lastUpdated=DateTime.Now;

            Models.StorePrice st = new Models.StorePrice();
            foreach (Boozic.ProductsPrice pr in lstTemp)
            {
                Store tmpStore = ss.GetById(pr.StoreID);



                double tmpDistance = ls.distance(SourceLatitude, SourceLongitude, (double)tmpStore.Latitude, (double)tmpStore.Longitude);

                if (tmpDistance < DistanceFromCurrentLocation)
                {
                    closestStoreId = (int)pr.StoreID;
                    DistanceFromCurrentLocation = tmpDistance;

                    ClosestStorePrice = (double)pr.Price;

                    Store closestStore = ss.GetById(closestStoreId);
                    st.StoreID = closestStore.Id;
                    st.StoreName = closestStore.StoreName;
                    st.StoreAddress = closestStore.Address;
                    st.Latitude = (double)closestStore.Latitude;
                    st.Longitude = (double)closestStore.Longitude;

                    //st = new Models.StorePrice(ls.getStores((double)tmpStore.Latitude, (double)tmpStore.Longitude, 0.2)[0]);
                    st.Distance = DistanceFromCurrentLocation;
                    //st.Duration = distanceResult["Duration"];
                    st.Price = (double)ClosestStorePrice;
                    st.LastUpdated = Convert.ToDateTime(pr.LastUpdated).ToString("MM/dd/yy");
                    st.StoreID = (int)pr.StoreID;
                }

            }



            return st;
        }

        //static void CopyProperties(object dest, object src)
        //{
        //    foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(src))
        //    {
        //        item.SetValue(dest, item.GetValue(src));
        //    }
        //}

        public String UpdateProduct(int ProductId, int StoreId = -1, double Price = -1, string ProductName = "-1", int ProductTypeId = -1, double ABV = -1,
                                    double Volume = -1, string VolumeUnit = "-1", string ContainerType = "-1", int ContainerQty = -1, string DeviceId = "-1",
                                    int Rating = -1, int AddToFavouritesList = -1)
        {
            String returnMessage = "Completed Succesfully";
            bool sendNotificaitons = false;
            string GCMmessage = "Sale Price for $";
            try
            {

                Product aProduct = sdContext.Products.SingleOrDefault(x => x.Id == (int)ProductId);
                if (ABV > -1)
                    aProduct.ABV = (decimal)ABV;
                if (Volume > -1)
                    aProduct.Volume = (decimal)Volume;
                if (VolumeUnit != "-1")
                    aProduct.VolumeUnit = VolumeUnit;
                if (ContainerType != "-1")
                    aProduct.ContainerType = ContainerType;
                if (ContainerQty != -1)
                    aProduct.ContainerQty = ContainerQty;
                if (ProductName != "-1")
                    aProduct.Name = ProductName;
                if (ProductTypeId > -1)
                    aProduct.TypeDetailsId = ProductTypeId;

                if (StoreId > -1 & Price > -1)
                {
                    ProductsPrice aProductPrice = sdContext.ProductsPrices.SingleOrDefault(x => x.ProductId == (int)ProductId && x.StoreID == (int)StoreId);
                    if (aProductPrice != null)
                    {
                        if (aProductPrice.Price > (decimal)Price)
                        {
                            sendNotificaitons = true;
                            GCMmessage += Price;

                        }
                        aProductPrice.Price = (decimal)Price;
                        aProductPrice.LastUpdated = DateTime.Now;
                    }
                    else
                    {
                        ProductsPrice tmpProductPrice = new ProductsPrice(); ;
                        tmpProductPrice.ProductId = ProductId;
                        tmpProductPrice.StoreID = StoreId;
                        tmpProductPrice.Price = (decimal)Price;
                        tmpProductPrice.LastUpdated = DateTime.Now;
                        sdContext.ProductsPrices.Add(tmpProductPrice);
                    }
                }

                if (DeviceId != "-1")
                {
                    if (Rating > -1)
                    {
                        UserProductRating aUserProductRating = sdContext.UserProductRatings.SingleOrDefault(x => x.ProductId == (int)ProductId && x.DeviceId == DeviceId);
                        if (aUserProductRating != null)
                            aUserProductRating.Rating = (int)Rating;
                        else
                        {
                            aUserProductRating = new UserProductRating();
                            aUserProductRating.DeviceId = DeviceId;
                            aUserProductRating.Rating = Rating;
                            aUserProductRating.ProductId = ProductId;
                            sdContext.UserProductRatings.Add(aUserProductRating);
                        }


                        ProductRating aProductRating = sdContext.ProductRatings.SingleOrDefault(x => x.ProductId == (int)ProductId);
                        if (aProductRating != null)
                        {
                            if (Rating == 1)
                                aProductRating.Rating1 += 1;
                            if (Rating == 2)
                                aProductRating.Rating2 += 1;
                            if (Rating == 3)
                                aProductRating.Rating3 += 1;
                            if (Rating == 4)
                                aProductRating.Rating4 += 1;
                            if (Rating == 5)
                                aProductRating.Rating5 += 1;

                            aProductRating.CombinedRating = (decimal)findAverageRating((int)aProductRating.Rating1, (int)aProductRating.Rating2, (int)aProductRating.Rating3, (int)aProductRating.Rating4, (int)aProductRating.Rating5);
                        }
                        else
                        {
                            aProductRating = new ProductRating();
                            aProductRating.ProductId = ProductId;
                            aProductRating.Rating1 = 0;
                            aProductRating.Rating2 = 0;
                            aProductRating.Rating3 = 0;
                            aProductRating.Rating4 = 0;
                            aProductRating.Rating5 = 0;
                            aProductRating.CombinedRating = 0;

                            if (Rating == 1)
                                aProductRating.Rating1 = 1;
                            if (Rating == 2)
                                aProductRating.Rating2 = 1;
                            if (Rating == 3)
                                aProductRating.Rating3 = 1;
                            if (Rating == 4)
                                aProductRating.Rating4 = 1;
                            if (Rating == 5)
                                aProductRating.Rating5 = 1;

                            aProductRating.CombinedRating = (decimal)findAverageRating((int)aProductRating.Rating1, (int)aProductRating.Rating2, (int)aProductRating.Rating3, (int)aProductRating.Rating4, (int)aProductRating.Rating5);
                            sdContext.ProductRatings.Add(aProductRating);
                        }
                    }

                    if (AddToFavouritesList == 1)
                    {

                        List<UserFavourite> lstFav = sdContext.UserFavourites.ToList().FindAll(delegate(UserFavourite s) { return s.DeviceId == DeviceId && s.ProductId == ProductId; });
                        if (lstFav.Count == 0)
                        {
                            UserFavourite f = new UserFavourite();
                            f.ProductId = ProductId;
                            f.DeviceId = DeviceId;
                            sdContext.UserFavourites.Add(f);

                        }
                    }

                }


                sdContext.SaveChanges();


            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }

            if (sendNotificaitons)
            {
                GCMService gs = new GCMService(new GCMRepository(new BoozicEntities()));
                List<UserFavourite> lstFav = sdContext.UserFavourites.ToList().FindAll(delegate(UserFavourite s) { return s.ProductId == ProductId; });
                foreach (UserFavourite fav in lstFav)
                {
                    GCMRegKey gcm = sdContext.GCMRegKeys.FirstOrDefault(delegate(GCMRegKey s) { return s.DeviceId == fav.DeviceId; });
                    Store st = sdContext.Stores.FirstOrDefault(delegate(Store s) { return s.Id == StoreId; });
                    GCMmessage += " at " + st.StoreName + "\n " + st.Address;
                    gs.SendNotification(GCMmessage, gcm.RegistrationToken);
                }





            }
            return returnMessage;
        }


        private double findAverageRating(int rating1, int rating2, int rating3, int rating4, int rating5)
        {
            double wTotal = rating1 * 1 + rating2 * 2 + rating3 * 3 + rating4 * 4 + rating5 * 5;
            double total = rating1 + rating2 + rating3 + rating4 + rating5;

            return Math.Round((wTotal / total), 2);
        }

        public Dictionary<int, string> getParentTypes()
        {
            Dictionary<int, string> ty = new Dictionary<int, string>();
            List<Type> tmp = sdContext.Types.ToList();
            foreach (Type t in tmp)
            {
                ty.Add(t.Id, t.Type1);
            }
            return ty;
        }

        public Dictionary<int, string> getProductTypes(int ParentId)
        {
            Dictionary<int, string> ty = new Dictionary<int, string>();
            List<TypesDetail> tmp = sdContext.TypesDetails.ToList().FindAll(delegate(TypesDetail s) { return s.ParentId == ParentId; });
            foreach (TypesDetail t in tmp)
            {
                ty.Add(t.Id, t.Description);
            }
            return ty;


        }

        public List<Models.ProductInfo> getFavourites(string DeviceId, double latitude, double longitude)
        {
            List<UserFavourite> lstFav = sdContext.UserFavourites.ToList().FindAll(delegate(UserFavourite s) { return s.DeviceId == DeviceId; });
            List<Models.ProductInfo> lstProductInfo = new List<Models.ProductInfo>();
            StoreService ss = new StoreService(new StoreRepository(new BoozicEntities()));

            foreach (UserFavourite f in lstFav)
            {
                Product p = sdContext.Products.SingleOrDefault(x => x.Id == (int)f.ProductId);
                Models.ProductInfo tmpProductInfo = new Models.ProductInfo();

                tmpProductInfo.ProductId = p.Id;
                tmpProductInfo.ProductName = p.Name;
                tmpProductInfo.ProductTypeId = p.TypeDetailsId;
                tmpProductInfo.UPC = p.UPC;
                tmpProductInfo.Volume = (double)p.Volume.GetValueOrDefault();
                tmpProductInfo.VolumeUnit = p.VolumeUnit;
                tmpProductInfo.ContainerType = p.ContainerType;
                tmpProductInfo.ContainerQty = (int)p.ContainerQty;

                tmpProductInfo.ABV = (double)p.ABV.GetValueOrDefault();
                tmpProductInfo.IsFoundInDatabase = 0;

                TypesDetail tmpDetails = sdContext.TypesDetails.SingleOrDefault(x => x.Id == (int)p.TypeDetailsId);
                tmpProductInfo.ProductType = tmpDetails.Description;

                Type tmpParentType = sdContext.Types.SingleOrDefault(x => x.Id == (int)tmpDetails.ParentId);
                tmpProductInfo.ProductParentTypeId = tmpParentType.Id;
                tmpProductInfo.ProductParentType = tmpParentType.Type1;

                ProductRating tmpRating = sdContext.ProductRatings.SingleOrDefault(x => x.ProductId == (int)p.Id);
                if (tmpRating != null)
                {
                    tmpProductInfo.Rating1 = (int)tmpRating.Rating1;
                    tmpProductInfo.Rating2 = (int)tmpRating.Rating2;
                    tmpProductInfo.Rating3 = (int)tmpRating.Rating3;
                    tmpProductInfo.Rating4 = (int)tmpRating.Rating4;
                    tmpProductInfo.Rating5 = (int)tmpRating.Rating5;
                    tmpProductInfo.CombinedRating = (double)tmpRating.CombinedRating;
                }
                else
                {
                    tmpProductInfo.Rating1 = 0;
                    tmpProductInfo.Rating2 = 0;
                    tmpProductInfo.Rating3 = 0;
                    tmpProductInfo.Rating4 = 0;
                    tmpProductInfo.Rating5 = 0;
                    tmpProductInfo.CombinedRating = 0;
                }


                UserProductRating aUserProductRating = sdContext.UserProductRatings.SingleOrDefault(x => x.ProductId == (int)p.Id && x.DeviceId == DeviceId);
                if (aUserProductRating != null)
                    tmpProductInfo.RatingByCurrentUser = (int)aUserProductRating.Rating;



                tmpProductInfo.CheapestStore = getCheapestStore((int)p.Id, latitude, longitude);
                tmpProductInfo.ClosestStore = getClosestStore((int)p.Id, latitude, longitude);
                if (tmpProductInfo.ClosestStore.StoreID > 0 && tmpProductInfo.CheapestStore.StoreID > 0 && tmpProductInfo.ClosestStore.StoreID == tmpProductInfo.CheapestStore.StoreID)
                    tmpProductInfo.IsClosestStoreAndCheapestStoreSame = true;
                else
                    tmpProductInfo.IsClosestStoreAndCheapestStoreSame = false;
                lstProductInfo.Add(tmpProductInfo);
            }


           //Default Radius =2 Miles
            lstProductInfo = lstProductInfo.FindAll(delegate(Models.ProductInfo s) { return s.CheapestStore.Distance <= 10 || s.ClosestStore.Distance <= 10; });

            return lstProductInfo;
        }


        public String addToFavourites(string DeviceId, int ProductId)
        {
            String returnMessage = "Completed Succesfully";
            try
            {
                List<UserFavourite> lstFav = sdContext.UserFavourites.ToList().FindAll(delegate(UserFavourite s) { return s.DeviceId == DeviceId && s.ProductId == ProductId; });
                if (lstFav.Count == 0)
                {
                    UserFavourite f = new UserFavourite();
                    f.ProductId = ProductId;
                    f.DeviceId = DeviceId;
                    sdContext.UserFavourites.Add(f);
                    sdContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return returnMessage;
        }

        public String deleteFromFavourites(string DeviceId, string ProductIds)
        {
            String returnMessage = "Completed Succesfully";
            string[] arrProductIds = ProductIds.Split(',');

            for (int i = 0; i < arrProductIds.Length; i++)
            {
                try
                {

                    UserFavourite f = sdContext.UserFavourites.ToList().FirstOrDefault(s => s.DeviceId == DeviceId && s.ProductId == Convert.ToInt32(arrProductIds[i]));
                    if (f != null)
                    {
                        sdContext.UserFavourites.Remove(f);
                        sdContext.SaveChanges();
                    }

                }
                catch (Exception ex)
                {
                    returnMessage += ex.Message;
                }
            }
            return returnMessage;
        }

        public String flagProduct(string DeviceId, int ProductId, int ReasonId)
        {
            String returnMessage = "Completed Succesfully";
            try
            {
                List<ProductFlag> lstFlg = sdContext.ProductFlags.ToList().FindAll(delegate(ProductFlag s) { return s.DeviceId == DeviceId && s.ProductId == ProductId; });
                if (lstFlg.Count == 0)
                {
                    ProductFlag f = new ProductFlag();
                    f.ProductId = ProductId;
                    f.DeviceId = DeviceId;
                    f.FlaggedOn = DateTime.Now;
                    f.Reason = ReasonId;
                    sdContext.ProductFlags.Add(f);
                    sdContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            return returnMessage;
            
       }
    }
}