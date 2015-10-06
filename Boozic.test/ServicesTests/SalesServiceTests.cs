using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Boozic.Repositories;
using Moq;
using Boozic.Services;
using Boozic;
using System.Collections.Generic;


namespace Boozic.test
{
    [TestClass]
    public class SalesServiceTests
    {
        readonly Mock<ISalesRepsitory> aMockRepository = new Mock<ISalesRepsitory>();

        private static readonly vwSale sale1 = new vwSale {SaleId=1, Price=10};
        private static readonly vwSale sale2 = new vwSale { SaleId = 2, Price = 20 };

        private List<vwSale> saleList = new List<vwSale>
        {
            sale1,
            sale2
        };

        [TestMethod]
        public void getSales_shouldPass()
        {
            // Arrange
            SalesService service = new SalesService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.getSales(2,1,1,1,1)).Returns(saleList);

            // Act
            var result = service.getSales(2, 1, 1, 1, 1);

            // Assert
            Assert.IsTrue(result.Count == 2);
        }

        [TestMethod]
        public void getSales_shouldFail()
        {
            // Arrange
            SalesService service = new SalesService(aMockRepository.Object);
            aMockRepository.Setup(aService => aService.getSales(1, 1, 1, 1, 1)).Returns(saleList);

            // Act
            var result = service.getSales(1, 0, 1, 1, 1);

            // Assert
            Assert.IsTrue(result.Count == 2);
        }
    }
}
