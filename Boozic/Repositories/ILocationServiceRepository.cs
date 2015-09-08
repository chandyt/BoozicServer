using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boozic.Models;

namespace Boozic.Repositories
{
   public interface ILocationServiceRepository
    {
        List<StoreInfo> getStores(double Latitude, double Longitude, double Radius);
      
      
    }
}
