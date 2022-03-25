using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using LondonStockApi.Service.Interfaces;
using LondonStockAPI;
using LondonStockAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace LondonStock.API
{
    [DependencyInjectionConfig(typeof(Startup))]
    public static class GetStockValueBySymbol
    {
        [FunctionName("GetStockValueBySymbol")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get value of stock using ticker symbols" })]
        [OpenApiParameter(name: "tickerSymbol", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **tickerSymbol** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(ExchangeTransactionResponse), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Inject] ILondonStockService londonStockService,
            ILogger log)
        {
            log.LogInformation("get current privce for ticker symbol");

            string tickerSymbol = req.Query["tickerSymbol"];

            var result = await londonStockService.GetCurrentValueBySymbol(tickerSymbol);

            if (result == null)
            {
                return new NotFoundObjectResult("no price found for symbol");
            }

            return new OkObjectResult(result);
        }
    }
}

