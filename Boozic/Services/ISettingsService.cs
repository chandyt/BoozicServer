using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    interface ISettingsService
    {
        Setting GetById(int aSiteSettingId);
        IEnumerable<Setting> GetAll();
        String GetGoogleAPIKey();
        String GetUPCAPIKey();
    }
}
