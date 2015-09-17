using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    public interface ISalesService
    {
        List<vwSale> getSales(int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 0, int LowestPrice = 0, int HighestPrice = 9999999);
    }
}
