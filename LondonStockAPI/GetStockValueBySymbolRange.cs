using System.Collections.Generic;
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
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace LondonStock.API
{
    [DependencyInjectionConfig(typeof(Startup))]
    public static class GetStockValueBySymbolRange
    {

        private const string contentType = "application/json";

        [FunctionName("GetStockValueBySymbolRange")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Get stock value buy ticker symbol range (comma separated list)" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "tickerSymbols", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **tickerSymbols** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: contentType, bodyType: typeof(IList<ExchangeTransactionResponse>), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Inject] ILondonStockService londonStockService,
            ILogger log)
        {
            log.LogInformation("Get stock value by ticker symbol range");

            string tickerSymbols = req.Query["tickerSymbols"];

            var results = await londonStockService.GetCurrentValueBySymbolRange(tickerSymbols);

            if (results == null)
            {
                return new NotFoundObjectResult("no prices found for symbol");
            }

            return new OkObjectResult(results);
        }
    }
}

