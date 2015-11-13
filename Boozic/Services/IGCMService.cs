using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    public interface IGCMService
    {
        void Add(GCMRegKey aRegKey);
        void Update(GCMRegKey aRegKey);
        GCMRegKey GetByDeviceID(string DeviceId);
        string SendNotification( string message);

        string SendEmail(string emailBody);
    }
}
