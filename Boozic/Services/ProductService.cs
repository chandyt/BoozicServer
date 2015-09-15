using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boozic.Repositories;

namespace Boozic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository repository;
       // private readonly ISettingsService appSettingsService;
        public ProductService(IProductsRepository aRepository)
        {
            repository = aRepository;
        }

    
        public ProductService()
        {
           // appSettingsService = new SettingsService(new SettingsRepository(new BoozicEntities()));
            
        }

        public Product GetById(int aProductID)
        {
          return  repository.GetById(aProductID);
        }
        public Models.ProductInfo GetByUPC(string aUPC)
        {
            return repository.GetByUPC(aUPC);

        }
    }
}