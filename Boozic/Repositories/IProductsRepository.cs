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
      }

}
