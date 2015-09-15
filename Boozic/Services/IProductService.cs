using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    interface IProductService
    {
        Product GetById(int aProductID);
        Models.Product GetByUPC(string aUPC);
    }
}
