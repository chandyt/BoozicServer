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
        String getProductName(string aUPC);
    }
}
