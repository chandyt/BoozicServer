using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boozic.Models;

namespace Boozic.Services
{
    public interface IStoreService
    {
        Store GetById(int aStoreID);
        List<StoreInfo> getStoresInRadius(double CurrentLatitude, double CurrentLongitude, double Radius);
    }
}
