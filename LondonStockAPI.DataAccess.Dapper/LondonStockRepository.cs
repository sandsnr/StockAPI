using Dapper;
using LondonStockAPI.DataAccess.Dapper.Interfaces;
using LondonStockAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LondonStockAPI.DataAccess.Dapper
{
    public class LondonStockRepository : ILondonStockRepository
    {
        private readonly DatabaseConfiguration databaseConfiguration;
        private readonly IDatabaseConnectionFactory databaseConnectionFactory;
        private const string insertProcedure = "[Insert_ExchangeTransaction]";
        private const string getByTickerSymbol = "[GetStockValue_ByTickerSymbol]";
        private const string getByTickerSymbolRange = "[GetStockValue_ByTickerSymbolRange]";

        public LondonStockRepository(DatabaseConfiguration databaseConfiguration, IDatabaseConnectionFactory databaseConnectionFactory)
        {
            if (databaseConfiguration == null || string.IsNullOrEmpty(databaseConfiguration.ConnectionString))
            {
                throw new ArgumentNullException(nameof(databaseConfiguration));
            }

            this.databaseConfiguration = databaseConfiguration;
            this.databaseConnectionFactory = databaseConnectionFactory;
        }

        public async Task<OperationResult> CreateExchangeTransactionsAsync(ExchangeTransactionRequest exchangeTransaction)
        {

            if (exchangeTransaction == null)
            {
                return OperationResult.Failure("Cannot create exchange transaction from a null object");
            }

            try
            {

                using var connection = databaseConnectionFactory.GetConnection(databaseConfiguration.ConnectionString);
                var values = new
                {
                    exchangeTransaction.BrokerId,
                    exchangeTransaction.TickerSymbol,
                    exchangeTransaction.Price,
                    exchangeTransaction.NumberOfShares
                };
                var results = await connection.QueryAsync(insertProcedure, values, commandType: CommandType.StoredProcedure);

                return OperationResult.Success();
            }
            catch (Exception exception)
            {
                return OperationResult.Failure(string.Format("Cannot create exchange transaction: {0}", exception.Message));
            }
        }

        public async Task<OperationResult<ExchangeTransactionResponse>> GetCurrentValueBySymbol(string tickerSymbol)
        {
            if (string.IsNullOrWhiteSpace(tickerSymbol))
            {
                return OperationResult<ExchangeTransactionResponse>.Failure("Ticker symbol cannot be null or empty");
            }

            try
            {

                using var connection = databaseConnectionFactory.GetConnection(databaseConfiguration.ConnectionString);
                var values = new
                {
                    TickerSymbol = tickerSymbol
                };
                var results = await connection.QuerySingleAsync<ExchangeTransactionResponse>(getByTickerSymbol, values, commandType: CommandType.StoredProcedure);

                return OperationResult<ExchangeTransactionResponse>.Success(data: results);
            }
            catch (Exception exception)
            {
                return OperationResult<ExchangeTransactionResponse>.Failure(string.Format("Cannot get symbol price: {0}", exception.Message));
            }
        }

        public async Task<OperationResult<IList<ExchangeTransactionResponse>>> GetCurrentValueBySymbolRange(string tickerSymbols)
        {
            if (string.IsNullOrWhiteSpace(tickerSymbols))
            {
                return OperationResult<IList<ExchangeTransactionResponse>>.Failure("Ticker symbols cannot be null or empty");
            }

            try
            {

                using var connection = databaseConnectionFactory.GetConnection(databaseConfiguration.ConnectionString);
                var values = new
                {
                    TickerSymbols = tickerSymbols
                };
                var results = await connection.QueryAsync<ExchangeTransactionResponse>(getByTickerSymbolRange, values, commandType: CommandType.StoredProcedure);

                return OperationResult<IList<ExchangeTransactionResponse>>.Success(data: results.ToList());
            }
            catch (Exception exception)
            {
                return OperationResult<IList<ExchangeTransactionResponse>>.Failure(string.Format("Cannot get symbol price: {0}", exception.Message));
            }
        }
    }
}
