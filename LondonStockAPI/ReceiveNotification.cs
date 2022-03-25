using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using LondonStockApi.Service.Interfaces;
using LondonStockAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace LondonStockAPI
{
    [DependencyInjectionConfig(typeof(Startup))]
    public static class ReceiveNotification
    {
        private const string contentType = "application/json";

        [FunctionName("receive-notification")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Create notification of trades" })]
        [OpenApiRequestBody(contentType: contentType, bodyType: typeof(ExchangeTransactionRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: contentType, bodyType: typeof(IList<OperationResult>), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Inject] ILondonStockService londonStockService,
            ILogger log)
        {
            log.LogInformation("Calling receive notification");


            var exchangeTransactionRequest = JsonConvert.DeserializeObject<ExchangeTransactionRequest>(await new StreamReader(req.Body).ReadToEndAsync());


            var operationResult = await londonStockService.CreateExchangeTransactionsAsync(exchangeTransactionRequest).ConfigureAwait(false);

            if (operationResult.Status)
            {
                return new OkObjectResult(operationResult.Message);
            }

            return new BadRequestObjectResult(operationResult.Message);
        }
    }
}

