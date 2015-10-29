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
        Models.ProductInfo GetByUPC(string aUPC);
        Models.ProductInfo getProductUsingAPI(string aUPC);

        void addProduct(Product aProduct);

        List<Models.ProductInfo> filterProducts(decimal latitude, decimal longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 0, int LowestPrice = 0,
                                    int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                    bool SortByProductType = false, bool SortByDistance = false, bool SortByPrice = true, bool SortByRating = false,
                                    bool SortAscending = true);
    }
}
