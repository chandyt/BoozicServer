using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;

namespace Boozic.Services
{
    public class StoreService:IStoreService
    {
         private readonly IStoreRepository repository;
         public StoreService(IStoreRepository aRepository)
        {
            repository = aRepository;
        }
        public Store GetById(int aSiteSettingId)
        {
            return repository.GetById(aSiteSettingId);
        }
        public IEnumerable<Store> GetAll()
        {
            return repository.GetAll();
        }
    }
}