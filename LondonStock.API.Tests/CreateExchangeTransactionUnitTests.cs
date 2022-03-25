using LondonStock.API.Tests.Helpers;
using LondonStockApi.Service;
using LondonStockApi.Service.Interfaces;
using LondonStockAPI;
using LondonStockAPI.DataAccess.Dapper.Interfaces;
using LondonStockAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LondonStock.API.Tests
{
    public class CreateExchangeTransactionUnitTests : FunctionTest
    {

        private readonly Mock<ILondonStockRepository> londonStockRepository;
        private readonly ILondonStockService londonStockService;
        private readonly Mock<ILogger> logger;

        public CreateExchangeTransactionUnitTests()
        {
            londonStockRepository = new Mock<ILondonStockRepository>();
            londonStockService = new LondonStockService(londonStockRepository.Object);
            logger = new Mock<ILogger>();
        }

        [Test]
        public async Task ShouldReturnBadRequestRequestWhenInputModelIsInvalid()
        {
            // Arrange

            // Act
            var query = new Dictionary<String, StringValues>();
            var body = "{\"name\":\"yamada\"}";

            var result = await ReceiveNotification.Run(HttpRequestSetup(query, body), londonStockService, logger.Object);
            var resultObject = (BadRequestObjectResult)result;

            // Assert
            Assert.AreEqual(400, resultObject.StatusCode.Value);
        }

        [Test]
        public async Task ShouldReturnBadRequestRequestWhenPricelIsInvalid()
        {
            // Arrange

            // Act
            var query = new Dictionary<String, StringValues>();
            var exchangeTransaction = new ExchangeTransactionRequest()
            {
                BrokerId = Guid.NewGuid(),
                NumberOfShares = 100,
                TickerSymbol = "STA",
                Price = -110
            };
            var body = JsonConvert.SerializeObject(exchangeTransaction);

            var result = await ReceiveNotification.Run(HttpRequestSetup(query, body), londonStockService, logger.Object);
            var resultObject = (BadRequestObjectResult)result;

            // Assert
            Assert.AreEqual(400, resultObject.StatusCode);
        }

        [Test]
        public async Task ShouldReturnOkForValidExchangeTransaction()
        {
            // Arrange
            londonStockRepository.Setup(x => x.CreateExchangeTransactionsAsync(It.IsAny<ExchangeTransactionRequest>())).Returns(Task.FromResult(OperationResult.Success()));

            // Act
            var query = new Dictionary<String, StringValues>();
            var exchangeTransaction = new ExchangeTransactionRequest()
            {
                BrokerId = Guid.NewGuid(),
                NumberOfShares = 100,
                TickerSymbol = "STA",
                Price = 110
            };
            var body = JsonConvert.SerializeObject(exchangeTransaction);

            var result = await ReceiveNotification.Run(HttpRequestSetup(query, body), londonStockService, logger.Object);
            var resultObject = (OkObjectResult)result;

            // Assert
            Assert.AreEqual(200, resultObject.StatusCode.Value);
        }
    }
}