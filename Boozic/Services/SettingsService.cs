using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;

namespace Boozic.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingsRepository repository;
        public SettingsService(ISettingsRepository aRepository)
        {
            repository = aRepository;
        }
        public Setting GetById(int aSiteSettingId)
        {
            return repository.GetById(aSiteSettingId);
        }
        public IEnumerable<Setting> GetAll()
        {
            return repository.GetAll();
        }
        public String GetGoogleAPIKey()
        {
             return repository.GetById(1).Value; 
        }

        public String GetUPCAPIKey()
        {
            return repository.GetById(2).Value;
        }
    }
}