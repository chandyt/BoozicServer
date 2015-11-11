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
                                     int SortOption = 0, bool SortByCheapestStorePrice = false);

        String UpdateProduct(int ProductId, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId, int rating);

        String InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0);
    }
}
