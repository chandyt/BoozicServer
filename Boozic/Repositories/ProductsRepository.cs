using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Boozic.Repositories
{
    public class ProductsRepository : BaseContentRepository<Product>, IProductsRepository
    {
            public BoozicEntities sdContext { get; private set; }
            public ProductsRepository(BoozicEntities ctx)
            : base(ctx)
            {
                sdContext = ctx;

            }

            public IEnumerable<Product> GetAll()
            {
                return GetObjectSet();
            }

            public override DbSet<Product> GetObjectSet()
            {
                return sdContext.Products;
            }

            public override Product GetById(object id)
            {
                if (id is int)
                {
                    return sdContext.Products.SingleOrDefault(x => x.Id == (int)id);
                }
                return null;
            }
            public Models.ProductInfo GetByUPC(string UPC)
            {
                if (UPC != string.Empty)
                {
                    Product p = sdContext.Products.SingleOrDefault(x => x.UPC == (string)UPC);
                    Models.ProductInfo pr = new Models.ProductInfo();
                    if (pr.UPC != null)
                    {
                        pr.ProductId = p.Id;
                        pr.ProductName = p.Name;
                        pr.ProductTypeId = p.TypeId;
                        pr.ProductType = "Test"; //TODO: read from table
                        pr.UPC = p.UPC;
                        pr.SizeInfo = p.SizeInfo;
                        pr.IsFoundInDatabase = true;

                    }
                    else // TODO: Read from API
                    { 
                        
                    }
                    return pr;

                }
                return null;
            }



    }
}