using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boozic.Repositories;
using Moq;
using Boozic.Services;
using Boozic;
using Boozic.Controllers;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;


namespace Boozic.test.ControllerTests
{
    [TestClass]
    public class SalesControllerTests
    {
        private Mock<ISalesService> aMockSalesService;
        private static readonly vwSale sale1 = new vwSale {SaleId=1, Price=10};
        private static readonly vwSale sale2 = new vwSale { SaleId = 2, Price = 20 };

        private List<vwSale> saleList = new List<vwSale>
        {
            sale1,
            sale2
        };

        private FilterController controller;



        [TestInitialize]
        public void Init()
        {
            aMockSalesService = new Mock<ISalesService>();
           controller= new FilterController();
    
        }



        [TestMethod]
        public void getSalesTest()
        {
            aMockSalesService.Setup(aService => aService.getSales(1,1,1,1,1)).Returns(saleList);



            IHttpActionResult result = controller.getSales(1,1,1,1,1) as IHttpActionResult;
          //  result.GetType();
        }
    }
}
