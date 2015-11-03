using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class StoreRepository:  BaseContentRepository<Store>, IStoreRepository
    {
          public BoozicEntities sdContext { get; private set; }
          public StoreRepository(BoozicEntities ctx)
            : base(ctx)
            {
                sdContext = ctx;

            }

          public override Store GetById(object id)
           {
           if (id is int)
           {
                return sdContext.Stores.SingleOrDefault(x => x.Id == (int)id);
           }
           return null;
           }

          public IEnumerable<Store> GetAll()
        {
            return GetObjectSet();
        }

          public override DbSet<Store> GetObjectSet()
        {
            return sdContext.Stores;
        }
    }
}