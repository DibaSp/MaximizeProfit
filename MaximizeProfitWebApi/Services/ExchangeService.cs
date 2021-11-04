using MaximizeProfitLib;
using System;
using System.Linq;

namespace MaximizeProfitWebApi.Services
{
    public class ExchangeService : IExchangeService
    {
        public MetaExchange GetMetaExchange(string typeOfOrder)
        {
            Random rand = new Random();
            int numberOfExchanges = rand.Next(1, 10);

            decimal[] exchangeFounds = Enumerable.Range(0, numberOfExchanges).Select(r => (decimal)rand.Next(0, 10000)).ToArray();
            var factory = new OrderFactory(exchangeFounds);
            var metaExchange = new MetaExchange(factory.GetExchanges(typeOfOrder, "data/order_books_data"));

            return metaExchange;
        }
    }
}
