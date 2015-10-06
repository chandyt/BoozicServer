using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class SalesRepository : BaseContentRepository<vwSale>, ISalesRepsitory
    {
        public BoozicEntities sdContext { get; private set; }
        public SalesRepository(BoozicEntities ctx)
            : base(ctx)
            {
                sdContext = ctx;

            }

        public IEnumerable<vwSale> GetAll()
            {
                return GetObjectSet();
            }

        public override DbSet<vwSale> GetObjectSet()
            {
                return sdContext.vwSales;
            }

       public override vwSale GetById(object id)
            {
                if (id is int)
                {
                    return sdContext.vwSales.SingleOrDefault(x => x.SaleId == (int)id);
                }
                return null;
            }

       public List<vwSale> getSales(int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 0, int LowestPrice = 0, int HighestPrice = 9999999)
       {
           List<vwSale> Sales = GetAll().ToList();
           if (ProductTypeId > 0)
           {
               Sales = Sales.FindAll(s => s.ProductId.Equals(ProductTypeId));
           }
           if (ProductParentTypeId > 0)
           {
               Sales = Sales.FindAll(s => s.ProductParentTypeId.Equals(ProductParentTypeId));
           }
           if (LowestPrice > 0)
           {
               Sales = Sales.FindAll(delegate(vwSale s) { return s.Price > LowestPrice; });
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