using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class ProductsRepository : BaseContentRepository<Product>, ISettingsRepository
    {
            public BoozicEntities sdContext { get; private set; }
            public ProductsRepository(BoozicEntities ctx)
            : base(ctx)
            {
                sdContext = ctx;

            }
            public override Product GetById(object id)
            {
                if (id is int)
                {
                    return sdContext.Products.SingleOrDefault(x => x.Id == (int)id);
                }
                return null;
            }
            public Models.Product GetByUPC(string UPC)
            {
                if (UPC != string.Empty)
                {
                    Product p = sdContext.Products.SingleOrDefault(x => x.UPC == (string)UPC);
                    Models.Product pr = new Models.Product();
                    pr.ProductId = p.Id;
                    pr.ProductName = p.Name;
                    pr.ProductTypeId = p.TypeId;
                    pr.ProductType = ""; //TODO: read from table
                    pr.UPC = p.UPC;

                }
                return null;
            }
    }
}