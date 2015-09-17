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

    }
}