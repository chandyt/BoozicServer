﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boozic.Services
{
    public interface IProductService
    {
        Product GetById(int aProductID);
        Models.ProductInfo GetByUPC(string aUPC);
        Models.ProductInfo getProductUsingAPI(string aUPC);

        void addProduct(Product aProduct);

        List<Models.ProductInfo> filterProducts(double latitude, double longitude, int ProductTypeId = 0, int ProductParentTypeId = 0, int Radius = 2, int LowestPrice = 0,
                                    int HighestPrice = 9999999, int LowestRating = 0, int HighestRating = 5, int LowestABV = 0, int HighestABV = 100,
                                     int SortOption = 0);
    }
}
