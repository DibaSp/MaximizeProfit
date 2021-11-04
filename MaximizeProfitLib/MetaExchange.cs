using MaximizeProfitLib.Models;
using System;
using System.Collections.Generic;

namespace MaximizeProfitLib
{
    public class MetaExchange
    {
        public List<Exchange> Exchanges { get; set; }
        public MetaExchange(List<Exchange> exchanges)
        {
            Exchanges = exchanges;
        }

        public List<Order> GetBuyOrdersForAmount(decimal amount)
        {
            List<Order> buyOrders = new List<Order>();
            while (amount > 0)
            {
                Exchange exchange = GetExchangeWithBestOrder((price1, price2) => price1 < price2);
                if (exchange == null) break;

                amount = exchange.CalculatePossibleBuy(amount);

                buyOrders.Add(exchange.BestOrder);

                exchange.RemoveBestOrder();
            }

            return buyOrders;
        }
        public List<Order> GetSellOrdersForAmount(decimal amount)
        {
            List<Order> sellOrders = new List<Order>();

            while (amount > 0)
            {
                Exchange exchange = GetExchangeWithBestOrder((price1, price2) => price1 > price2);
                if (exchange == null) break;

                amount = exchange.CalculatePossibleSell(amount);

                sellOrders.Add(exchange.BestOrder);

                exchange.RemoveBestOrder();
            }

            return sellOrders;
        }

        private Exchange GetExchangeWithBestOrder(Func<decimal, decimal, bool> comparer)
        {
            Exchange exchange = null;
            for (int i = 0; i < Exchanges.Count; i++)
            {
                if (Exchanges[i].ExchangeFounds <= 0) continue;

                Exchange candidateExchange = Exchanges[i];

                if (candidateExchange.Book.Orders.Count == 0) continue;

                if (exchange == null)
                {
                    exchange = candidateExchange;
                    continue;
                }

                if (comparer(candidateExchange.BestOrder.Price, exchange.BestOrder.Price))
                {
                    exchange = candidateExchange;
                }
            }

            return exchange;
        }
    }

}
