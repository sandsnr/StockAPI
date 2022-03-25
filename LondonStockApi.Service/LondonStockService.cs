using LondonStockApi.Service.Interfaces;
using LondonStockAPI.DataAccess.Dapper.Interfaces;
using LondonStockAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LondonStockApi.Service
{
    public class LondonStockService : ILondonStockService
    {
        private readonly ILondonStockRepository londonStockRepository;

        public LondonStockService(ILondonStockRepository londonStockRepository)
        {
            if (londonStockRepository == null)
            {
                throw new ArgumentNullException(nameof(londonStockRepository));
            }



            this.londonStockRepository = londonStockRepository;
        }

        public async Task<OperationResult> CreateExchangeTransactionsAsync(ExchangeTransactionRequest exchangeTransactionRequest)
        {
            if (exchangeTransactionRequest == null)
            {
                return OperationResult.Failure("Cannot create exchange transaction for null object");
            }

            if (exchangeTransactionRequest.BrokerId == Guid.Empty)
            {
                return OperationResult.Failure(string.Format("{0} is required", nameof(exchangeTransactionRequest.BrokerId)));
            }

            if (string.IsNullOrWhiteSpace(exchangeTransactionRequest.TickerSymbol))
            {
                return OperationResult.Failure(string.Format("{0} cannot be null or empty", nameof(exchangeTransactionRequest.TickerSymbol)));
            }

            if (exchangeTransactionRequest.NumberOfShares <= 0)
            {
                return OperationResult.Failure(string.Format("{0} provided is invalid", nameof(exchangeTransactionRequest.NumberOfShares)));
            }

            if (exchangeTransactionRequest.Price <= 0)
            {
                return OperationResult.Failure(string.Format("{0} provided is invalid", nameof(exchangeTransactionRequest.Price)));
            }

            var operationResult = await londonStockRepository.CreateExchangeTransactionsAsync(exchangeTransactionRequest).ConfigureAwait(false);

            return operationResult;
        }

        public async Task<OperationResult<ExchangeTransactionResponse>> GetCurrentValueBySymbol(string tickerSymbol)
        {
            if (string.IsNullOrWhiteSpace(tickerSymbol))
            {
                return OperationResult<ExchangeTransactionResponse>.Failure(string.Format("{0} cannot be null or empty", nameof(tickerSymbol)));
            }

            return await londonStockRepository.GetCurrentValueBySymbol(tickerSymbol);
        }

        public async Task<OperationResult<IList<ExchangeTransactionResponse>>> GetCurrentValueBySymbolRange(string tickerSymbols)
        {

            if (string.IsNullOrWhiteSpace(tickerSymbols))
            {
                return OperationResult<IList<ExchangeTransactionResponse>>.Failure(string.Format("{0} cannot be null or empty", nameof(tickerSymbols)));
            }

            var results = await londonStockRepository.GetCurrentValueBySymbolRange(tickerSymbols);

            return results;
        }
    }
}
