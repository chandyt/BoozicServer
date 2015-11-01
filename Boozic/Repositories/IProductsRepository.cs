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

        List<vwProductsWithStorePrice> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                    int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                    bool SortByProductType = false, bool SortByDistance = false, bool SortByPrice = true, bool SortByRating = false,
                                    bool SortAscending = true);
      }

}
