using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boozic.Models;

namespace Boozic.Repositories
{
    public interface IStoreRepository : IObjectRepository<Store>
    {

        IEnumerable<Store> GetAll();
        List<StoreInfo> getStoresInRadius(double CurrentLatitude, double CurrentLongitude, double Radius);
    }
}
