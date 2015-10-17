using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Repositories
{
   public interface IGCMRepository : IObjectRepository<GCMRegKey>
    {
        IEnumerable<GCMRegKey> GetAll();
        void Add(GCMRegKey RegKey);

        GCMRegKey GetByDeviceID(string DeviceID);

    }
}
