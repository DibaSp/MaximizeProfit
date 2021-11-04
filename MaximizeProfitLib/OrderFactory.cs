using MaximizeProfitLib.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MaximizeProfitLib
{
    public class OrderFactory
    {

        public decimal[] ExchangeFounds { get; }
        public OrderFactory(params decimal[] exchangeFounds)
        {
            ExchangeFounds = exchangeFounds;
        }

        public List<Exchange> GetExchanges(string typeOfOrder, string inputFile)
        {
            List<Exchange> exchanges = new List<Exchange>();
            int i = 0;
            foreach (string line in File.ReadAllLines(inputFile))
            {
                if (exchanges.Count == ExchangeFounds.Length) break;

                var bookLine = line.Split('\t')[1];
                Book book = JsonSerializer.Deserialize<Book>(bookLine);
                book.SortOrders(typeOfOrder);
                exchanges.Add(new Exchange
                {
                    Book = book,
                    ExchangeFounds = ExchangeFounds[i],
                });
                i++;
            }

            return exchanges;
        }
    }
}
