using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class GCMRepository : BaseContentRepository<GCMRegKey>, IGCMRepository
    {
        public BoozicEntities sdContext { get; private set; }
        public GCMRepository(BoozicEntities ctx)
            : base(ctx)
        {
            sdContext = ctx;

        }

        public IEnumerable<GCMRegKey> GetAll()
        {
            return GetObjectSet();
        }

        public override DbSet<GCMRegKey> GetObjectSet()
        {
            return sdContext.GCMRegKeys;
        }

        public void Add(GCMRegKey aRegKey)
        {
            sdContext.GCMRegKeys.Add(aRegKey);
            sdContext.SaveChanges();
        }

        public void Update(GCMRegKey aRegKey)
        {
            GCMRegKey RegKey = GetById(aRegKey.Id);
            RegKey.RegistrationToken = aRegKey.RegistrationToken;
            sdContext.SaveChanges();
        }

        public GCMRegKey GetByDeviceID(string DeviceId)
        {
            return sdContext.GCMRegKeys.SingleOrDefault(x => x.DeviceId == DeviceId);
        }

        public override GCMRegKey GetById(object id)
        {
            if (id is int)
            {
                return sdContext.GCMRegKeys.SingleOrDefault(x => x.Id == (int)id);
            }
            return null;
        }
    }
}