using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    public interface IProductService
    {
        Product GetById(int aProductID);
        Models.ProductInfo GetByUPC(string aUPC, double latitude, double longitude);
        Models.ProductInfo getProductUsingAPI(string aUPC);

        void addProduct(Product aProduct);

        List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, double LowestPrice = 0,
                                    double HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, double LowestABV = 0, double HighestABV = 100,
                                     int SortOption = 0, bool SortByCheapestStorePrice = false, string DeviceId = "-1");

        String UpdateProduct(int ProductId, int StoreId = -1, double Price = -1, string ProductName = "-1", int ProductTypeId = -1, double ABV = -1, double Volume = -1, string VolumeUnit = "-1", string ContainerType = "-1", string DeviceId = "-1", int Rating = -1);

       // String InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0);

        Dictionary<int, string> getParentTypes();

        Dictionary<int, string> getProductTypes(int ParentTypeId);

        List<Models.ProductInfo> getFavourites(string DeviceId, double latitude, double longitude);

        String addToFavourites(string DeviceId, int ProductId);
    }
}
