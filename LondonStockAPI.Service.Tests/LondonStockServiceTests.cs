using LondonStockApi.Service;
using LondonStockApi.Service.Interfaces;
using LondonStockAPI.DataAccess.Dapper.Interfaces;
using LondonStockAPI.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LondonStockAPI.Service.Tests
{
    public class LondonStockServiceTests
    {
        private readonly Mock<ILondonStockRepository> londonStockRepository;
        private readonly ILondonStockService londonStockService;

        public LondonStockServiceTests()
        {
            londonStockRepository = new Mock<ILondonStockRepository>();
            londonStockService = new LondonStockService(londonStockRepository.Object);
        }

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShouldReturnAFailedOperationResultForANullExchangeTransactionRequest()
        {
            // Arrange


            // Act
            var sut = londonStockService.CreateExchangeTransactionsAsync(null);

            // Assert
            Assert.False(sut.Result.Status);
        }

        [Test]
        public void ShouldReturnAFailedOperationResultForWhenPriceIsNegative()
        {
            // Arrange
            var exchangetransaction = new ExchangeTransactionRequest()
            {
                BrokerId = Guid.NewGuid(),
                NumberOfShares = 100,
                TickerSymbol = "STA",
                Price = -110
            };

            // Act
            var sut = londonStockService.CreateExchangeTransactionsAsync(null);

            // Assert
            Assert.False(sut.Result.Status);
        }

        [Test]
        public void ShouldReturnSuccessfulOperationResultForAValidExchangeTransactionRequest()
        {
            // Arrange
            londonStockRepository.Setup(x => x.CreateExchangeTransactionsAsync(It.IsAny<ExchangeTransactionRequest>())).Returns(Task.FromResult(OperationResult.Success()));
            var exchangeTransaction = new ExchangeTransactionRequest()
            {
                BrokerId = Guid.NewGuid(),
                NumberOfShares = 100,
                TickerSymbol = "STA",
                Price = 110
            };

            // Act
            var sut = londonStockService.CreateExchangeTransactionsAsync(exchangeTransaction);

            // Assert
            Assert.True(sut.Result.Status);
        }
    }
}