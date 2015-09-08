using Boozic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    interface ILocationService
    {
        List<StoreInfo> getStores(double Latitude, double Longitude, double Radius);
    }
}
