using Boozic.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boozic.Services
{
    public class SalesService : ISalesService
    {
         private readonly ISalesRepsitory repository;
         public SalesService(ISalesRepsitory aRepository)
        {
            repository = aRepository;
        }

         public List<vwSale> getSales(int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 0, int LowestPrice = 0, int HighestPrice = 9999999)
        {
           return repository.getSales( ProductTypeId, ProductParentTypeId , Radius, LowestPrice , HighestPrice);


            //TODO: Calculate for radius
            //return Sales;
        }
    }
}