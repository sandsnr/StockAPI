using System;

namespace LondonStockAPI.Models
{
    public class ExchangeTransactionRequest
    {
        public Guid BrokerId { get; set; }

        public string TickerSymbol { get; set; }

        public decimal Price { get; set; }

        public decimal NumberOfShares { get; set; }
    }
}
