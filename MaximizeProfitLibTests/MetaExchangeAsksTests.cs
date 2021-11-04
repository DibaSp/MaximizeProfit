using FluentAssertions;
using MaximizeProfitLib;
using MaximizeProfitLib.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaximizeProfitLibTests
{
    public class MetaExchangeAsksTests
    {
        [Fact]
        public void TestWithOneExchange_UnlimitedFound()
        {
            decimal expectedAmount = 9;
            var exchangeAsks = new (int amount, int price)[] { (7, 3000), (4, 3300), (9, 3500) };
            Exchange exchange = getExchangeForAsks(exchangeAsks, 100000);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(expectedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeAsks.Take(1).Sum(e => e.price * e.amount) + (exchangeAsks[1].price * 2));
            orders.Sum(o => o.Amount).Should().Be(expectedAmount);
        }

        [Fact]
        public void TestWithOneExchange_WhenThereAreNotEnoughFounds()
        {
            decimal founds = 300;
            var exchangeAsks = new (int amount, int price)[] { (7, 3000), (4, 3300), (9, 3500) };
            Exchange exchange = getExchangeForAsks(exchangeAsks, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(9);

            orders.Sum(o => o.Price * o.Amount).Should().Be(founds);
            orders.Sum(o => o.Amount).Should().Be(0.1M);
        }

        [Fact]

        public void TestWithOneExchange_WhenThereAreNotEnoughFoundsToHaveABitLessThenFounds()
        {
            decimal founds = 100;
            var exchangeAsks = new (int amount, int price)[] { (7, 3000), (4, 3300), (9, 3500) };
            Exchange exchange = getExchangeForAsks(exchangeAsks, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(9);

            orders.Sum(o => o.Price * o.Amount).Should().BeLessThan(founds);
            orders.Sum(o => o.Amount).Should().Be(0.0333333333333333333333333333M);
        }

        [Fact]
        public void TestWithOneExchange_WhenNotEnoughAsks()
        {
            decimal founds = 999999;
            var exchangeAsks = new (int amount, int price)[] { (7, 3000), (4, 3300), (9, 3500) };
            Exchange exchange = getExchangeForAsks(exchangeAsks, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(21);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeAsks.Sum(e => e.price * e.amount));
            orders.Sum(o => o.Amount).Should().Be(20);
        }

        [Fact]
        public void TestWithOneExchange_WhenNoFoundShouldReturnZero()
        {
            decimal founds = 0;
            var exchangeAsks = new (int amount, int price)[] { (7, 3000), (4, 3300), (9, 3500) };
            Exchange exchange = getExchangeForAsks(exchangeAsks, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(21);

            orders.Count.Should().Be(0);
        }

        [Fact]
        public void TestWithOneExchange_WhenAsksAreNotOrdered()
        {
            decimal founds = 100000;
            decimal expectedAmount = 9;
            var exchangeAsks = new (int amount, int price)[] { (9, 3500), (7, 3000), (4, 3300), };
            Exchange exchange = getExchangeForAsks(exchangeAsks, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(expectedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be((exchangeAsks[1].price * exchangeAsks[1].amount) + (exchangeAsks[2].price * 2));
            orders.Sum(o => o.Amount).Should().Be(expectedAmount);
        }

        [Fact]
        public void TestWithMultipleExchange_WhenNotEnoughAsks()
        {
            decimal expectedAmount = 8;
            decimal founds1 = 999999;
            var exchangeAsks1 = new (int amount, int price)[] { (2, 3000), (3, 3300), (9, 3500) };
            Exchange exchange1 = getExchangeForAsks(exchangeAsks1, founds1);
            decimal founds2 = 999999;
            var exchangeAsks2 = new (int amount, int price)[] { (2, 3100), (3, 3200), (7, 3700) };
            Exchange exchange2 = getExchangeForAsks(exchangeAsks2, founds2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(expectedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeAsks2.Take(2).Sum(e => e.price * e.amount) + exchangeAsks1.Take(1).Sum(e => e.price * e.amount) + (1 * exchangeAsks1[1].price));
            orders.Sum(o => o.Amount).Should().Be(expectedAmount);
        }

        [Fact]
        public void TestWithMultipleExchange_WhenAllNotEnoughFounds()
        {
            decimal expectedAmount = 8;
            decimal founds1 = 6000;
            var exchangeAsks1 = new (int amount, int price)[] { (2, 3000), (3, 3300), (9, 3500) };
            Exchange exchange1 = getExchangeForAsks(exchangeAsks1, founds1);
            decimal founds2 = 3100;
            var exchangeAsks2 = new (int amount, int price)[] { (2, 3100), (3, 3200), (7, 3700) };
            Exchange exchange2 = getExchangeForAsks(exchangeAsks2, founds2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(expectedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be((exchangeAsks1[0].amount * exchangeAsks1[0].price) + (1 * exchangeAsks2[0].price));
            orders.Sum(o => o.Amount).Should().Be(3);
        }

        [Fact]
        public void TestWithMultipleExchange_WhenSomeNotEnoughFounds()
        {
            decimal expectedAmount = 6;
            decimal founds1 = 999999;
            var exchangeAsks1 = new (int amount, int price)[] { (2, 3000), (3, 3300), (9, 3500) };
            Exchange exchange1 = getExchangeForAsks(exchangeAsks1, founds1);
            decimal founds2 = 3100;
            var exchangeAsks2 = new (int amount, int price)[] { (2, 3100), (3, 3200), (7, 3700) };
            Exchange exchange2 = getExchangeForAsks(exchangeAsks2, founds2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetBuyOrdersForAmount(expectedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeAsks1.Take(2).Sum(e => e.amount * e.price) + (1 * exchangeAsks2[0].price));
            orders.Sum(o => o.Amount).Should().Be(expectedAmount);
        }

        private Exchange getExchangeForAsks((int amount, int price)[] ps, decimal founds)
        {
            Book book = new Book
            {
                Asks = ps.Select(ps => new Ask
                {
                    Order = new Order
                    {
                        Amount = ps.amount,
                        Price = ps.price,
                        Type = "Buy"
                    }
                }).ToList(),
                Bids = new List<Bid>(),
            };
            book.SortOrders("Buy");
            return new Exchange
            {
                Book = book,
                ExchangeFounds = founds
            };
        }
    }
}
