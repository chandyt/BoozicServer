using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class SettingsRepository : BaseContentRepository<Setting>, ISettingsRepository
    {
            public BoozicEntities sdContext { get; private set; }
            public SettingsRepository(BoozicEntities ctx)
            : base(ctx)
            {
                sdContext = ctx;

            }

           public override Setting GetById(object id)
           {
           if (id is int)
           {
                return sdContext.Settings.SingleOrDefault(x => x.Id == (int)id);
           }
           return null;
           }
        
        public IEnumerable<Setting> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<Setting> GetObjectSet()
        {
            return sdContext.Settings;
        }
    }
}
