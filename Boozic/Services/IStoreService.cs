using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    public interface IStoreService
    {
        Store GetById(int aStoreID);
    }
}
