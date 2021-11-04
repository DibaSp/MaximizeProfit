using FluentAssertions;
using MaximizeProfitLib;
using MaximizeProfitLib.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaximizeProfitLibTests
{
    public class MetaExchangeBidsTests
    {
        [Fact]
        public void TestWithOneExchange_UnlimitedFound()
        {
            decimal numberOfBTC = 11;
            var exchangeBids = new (int amount, int price)[] { (9, 3500), (4, 3300), (7, 3000) };
            Exchange exchange = getExchangeForBids(exchangeBids, 100000);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(numberOfBTC);

            orders.Sum(o => o.Price * o.Amount).Should().Be((exchangeBids[0].amount * exchangeBids[0].price) + (2 * exchangeBids[1].price));
            orders.Sum(o => o.Amount).Should().Be(numberOfBTC);
        }

        [Fact]
        public void TestWithOneExchange_WhenThereAreNotEnoughFounds()
        {
            decimal founds = 7;
            var exchangeBids = new (int amount, int price)[] { (9, 3500), (4, 3300), (7, 3000) };
            Exchange exchange = getExchangeForBids(exchangeBids, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(9);

            orders.Sum(o => o.Price * o.Amount).Should().Be(7 * exchangeBids[0].price);
            orders.Sum(o => o.Amount).Should().Be(founds);
        }

        [Fact]
        public void TestWithOneExchange_WhenNotEnoughBids()
        {
            decimal founds = 999999;
            var exchangeBids = new (int amount, int price)[] { (9, 3500), (4, 3300), (7, 3000) };
            Exchange exchange = getExchangeForBids(exchangeBids, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(21);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeBids.Sum(eb => eb.amount * eb.price));
            orders.Sum(o => o.Amount).Should().Be(20);
        }

        [Fact]
        public void TestWithOneExchange_WhenNoFoundShouldReturnZero()
        {
            decimal founds = 0;
            var exchangeBids = new (int amount, int price)[] { (9, 3500), (4, 3300), (7, 3000) };
            Exchange exchange = getExchangeForBids(exchangeBids, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(21);

            orders.Count.Should().Be(0);
        }

        [Fact]
        public void TestWithOneExchange_WhenBidsAreUnordered()
        {
            decimal founds = 99999;
            decimal expctedAmount = 5;
            var exchangeBids = new (int amount, int price)[] { (4, 3300), (9, 3500), (7, 3000) };
            Exchange exchange = getExchangeForBids(exchangeBids, founds);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(expctedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(expctedAmount * exchangeBids[1].price);
            orders.Sum(o => o.Amount).Should().Be(expctedAmount);
        }

        [Fact]
        public void TestWithMultipleExchange_TooManyFounds()
        {
            decimal expctedAmount = 14;
            decimal foundsEx1 = 99999;
            var exchangeBids1 = new (int amount, int price)[] { (4, 3300), (2, 3500), (7, 3000) };
            decimal foundsEx2 = 99999;
            var exchangeBids2 = new (int amount, int price)[] { (4, 3250), (5, 3450), (5, 2900) };
            Exchange exchange1 = getExchangeForBids(exchangeBids1, foundsEx1);
            Exchange exchange2 = getExchangeForBids(exchangeBids2, foundsEx2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(expctedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(exchangeBids1.Take(2).Sum(e => e.amount * e.price) + (exchangeBids2[1].amount * exchangeBids2[1].price) + (3 * exchangeBids2[0].price));
            orders.Sum(o => o.Amount).Should().Be(expctedAmount);
        }

        [Fact]
        public void TestWithMultipleExchange_AllExTooLittleFounds()
        {
            decimal expctedAmount = 8;
            decimal foundsEx1 = 1.5M;
            var exchangeBids1 = new (int amount, int price)[] { (4, 3300), (2, 3500), (7, 3000) };
            decimal foundsEx2 = 2;
            var exchangeBids2 = new (int amount, int price)[] { (4, 3250), (5, 3450), (5, 2900) };
            Exchange exchange1 = getExchangeForBids(exchangeBids1, foundsEx1);
            Exchange exchange2 = getExchangeForBids(exchangeBids2, foundsEx2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(expctedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be((foundsEx2 * exchangeBids2[1].price) + (foundsEx1 * exchangeBids1[1].price));
            orders.Sum(o => o.Amount).Should().Be(foundsEx1 + foundsEx2);
        }

        [Fact]
        public void TestWithMultipleExchange_OneExTooLittleFounds()
        {
            decimal expctedAmount = 8;
            decimal foundsEx1 = 99999;
            var exchangeBids1 = new (int amount, int price)[] { (4, 3300), (2, 3500), (7, 3000) };
            decimal foundsEx2 = 2;
            var exchangeBids2 = new (int amount, int price)[] { (4, 3250), (5, 3450), (5, 2900) };
            Exchange exchange1 = getExchangeForBids(exchangeBids1, foundsEx1);
            Exchange exchange2 = getExchangeForBids(exchangeBids2, foundsEx2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(expctedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be((foundsEx2 * exchangeBids2[1].price) + exchangeBids1.Take(2).Sum(e => e.amount * e.price));
            orders.Sum(o => o.Amount).Should().Be(expctedAmount);
        }

        [Fact]
        public void TestWithMultipleExchange_AllZeroFounds()
        {
            decimal expctedAmount = 8;
            decimal foundsEx1 = 0;
            var exchangeBids1 = new (int amount, int price)[] { (4, 3300), (2, 3500), (7, 3000) };
            decimal foundsEx2 = 0;
            var exchangeBids2 = new (int amount, int price)[] { (4, 3250), (5, 3450), (5, 2900) };
            Exchange exchange1 = getExchangeForBids(exchangeBids1, foundsEx1);
            Exchange exchange2 = getExchangeForBids(exchangeBids2, foundsEx2);
            MetaExchange metaExchange = new MetaExchange(new List<Exchange> { exchange1, exchange2 });

            List<Order> orders = metaExchange.GetSellOrdersForAmount(expctedAmount);

            orders.Sum(o => o.Price * o.Amount).Should().Be(0);
            orders.Sum(o => o.Amount).Should().Be(0);
        }

        private Exchange getExchangeForBids((int amount, int price)[] ps, decimal founds)
        {
            Book book = new Book
            {
                Bids = ps.Select(ps => new Bid
                {
                    Order = new Order
                    {
                        Amount = ps.amount,
                        Price = ps.price,
                        Type = "Buy"
                    }
                }).ToList(),
                Asks = new List<Ask>(),
            };
            book.SortOrders("Sell");

            return new Exchange
            {
                Book = book,
                ExchangeFounds = founds
            };
        }
    }
}
