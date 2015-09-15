using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Repositories
{
    interface IProductsRepository: IObjectRepository<Product>
    {
         IEnumerable<Product> GetAll();
         Models.Product GetByUPC(string UPC);
      }

}
