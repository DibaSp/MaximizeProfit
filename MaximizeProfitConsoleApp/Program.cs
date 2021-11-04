using MaximizeProfitLib;
using MaximizeProfitLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MaximizeProfitConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter balance constraints (order books will be loaded from provided tsv). Example: 1000, 50000, 2000 (if selling BTC) and 10, 12, 14 (if buying BTC):");
            decimal[] balanceConstraints = Console.ReadLine().Split(',').Select(bc => decimal.Parse(bc)).ToArray();

            Console.WriteLine("Enter order type (buy, sell):");
            string typeOfOrder = Console.ReadLine(); 

            Console.WriteLine("Enter size of order (in BTC):");
            decimal sizeOfOrder = decimal.Parse(Console.ReadLine());

            ExecuteTrade(balanceConstraints, typeOfOrder, sizeOfOrder);
            
        }

        private static void ExecuteTrade(decimal[] balanceConstraints, string typeOfOrder, decimal sizeOfOrder)
        {
            var factoryBuy = new OrderFactory(balanceConstraints);
            var metaExchangeBuy = new MetaExchange(factoryBuy.GetExchanges(typeOfOrder, "data/order_books_data"));
            List<Order> optimalBuyOrders;
            if (typeOfOrder.ToLowerInvariant().Trim().Equals("buy"))
                optimalBuyOrders = metaExchangeBuy.GetBuyOrdersForAmount(sizeOfOrder);
            else
                optimalBuyOrders = metaExchangeBuy.GetSellOrdersForAmount(sizeOfOrder);

            decimal amount = 0, price = 0;
            foreach (var order in optimalBuyOrders)
            {
                amount += order.Amount;
                price += order.Amount * order.Price;
            }
            Console.WriteLine($"It's possible to {typeOfOrder.ToLowerInvariant().Trim()} {amount} of BTC for a price of {price} USD");
            Console.WriteLine("Execute orders:");

            Console.WriteLine(JsonSerializer.Serialize(optimalBuyOrders, new JsonSerializerOptions
            {
                WriteIndented = true,
            }));
        }
    }
}
