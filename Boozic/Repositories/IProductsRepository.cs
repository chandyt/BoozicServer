using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Repositories
{
    public interface IProductsRepository: IObjectRepository<Product>
    {
         IEnumerable<Product> GetAll();
         Models.ProductInfo GetByUPC(string UPC);

        void addProduct(Product aProduct);

        List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                    int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                     int SortOption = 0, bool SortByCheapestStorePrice = false);

        String UpdateProduct(int ProductId, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId, int rating);

        String InsertProduct(string UPC, string ProductName, int ProductTypeID, int StoreId, double Price, double ABV, double Volume, string VolumeUnit, string ContainerType, string DeviceId = "", int Rating = 0);
      }

}
