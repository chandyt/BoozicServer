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
            List<vwSale> Sales= repository.GetAll().ToList();
            if (ProductTypeId > 0)
            {
                Sales= Sales.FindAll(s => s.ProductId.Equals(ProductTypeId));
            }
            if (ProductParentTypeId > 0)
            {
                Sales = Sales.FindAll(s => s.ProductParentTypeId.Equals(ProductParentTypeId));
            }
            if (LowestPrice > 0)
            {
                Sales = Sales.FindAll(delegate(vwSale s) { return s.Price>LowestPrice;});
            }

            if (LowestPrice < 9999999)
            {
                Sales = Sales.FindAll(delegate(vwSale s) { return s.Price < HighestPrice; });
            }

            //TODO: Calculate for radius
            return Sales;
        }
    }
}