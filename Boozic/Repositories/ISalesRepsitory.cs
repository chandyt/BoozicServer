using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Repositories
{
    public interface ISalesRepsitory : IObjectRepository<vwSale>
    {
        IEnumerable<vwSale> GetAll();
        
    }
}
