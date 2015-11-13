﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Boozic.Services;
using Boozic.Models;
using System.ComponentModel;

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

          public List<StoreInfo> getStoresInRadius(double CurrentLatitude, double CurrentLongitude, double Radius)
          {
              List<Store> lstStores = GetAll().ToList();
              List<StoreInfo> lstSI = new List<StoreInfo>();
               LocationService ls = new LocationService();
              foreach (Store st in lstStores)
              {
                  double Distance = ls.distance(CurrentLatitude, CurrentLongitude, (double)st.Latitude, (double)st.Longitude);
                  if (Distance<= Radius)
                  {
                      StoreInfo SI = new StoreInfo(st);
                      SI.Distance = Distance;
                      lstSI.Add(SI);
                  }
                  
              }

              return lstSI;
          
          }
    }
}