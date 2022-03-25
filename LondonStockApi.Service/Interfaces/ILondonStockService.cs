using LondonStockAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LondonStockApi.Service.Interfaces
{
    public interface ILondonStockService
    {
        Task<OperationResult> CreateExchangeTransactionsAsync(ExchangeTransactionRequest exchangeTransactionRequests);
        Task<OperationResult<ExchangeTransactionResponse>> GetCurrentValueBySymbol(string tickerSymbol);
        Task<OperationResult<IList<ExchangeTransactionResponse>>> GetCurrentValueBySymbolRange(string tickerSymbols);
    }
}
