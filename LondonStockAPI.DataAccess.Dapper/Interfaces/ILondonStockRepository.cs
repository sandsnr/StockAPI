using LondonStockAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LondonStockAPI.DataAccess.Dapper.Interfaces
{
    public interface ILondonStockRepository
    {

        Task<OperationResult> CreateExchangeTransactionsAsync(ExchangeTransactionRequest exchangeTransaction);
        Task<OperationResult<ExchangeTransactionResponse>> GetCurrentValueBySymbol(string tickerSymbol);
        Task<OperationResult<IList<ExchangeTransactionResponse>>> GetCurrentValueBySymbolRange(string tickerSymbols);
    }
}
