using MaximizeProfitLib.Models;
using System.Linq;

namespace MaximizeProfitLib
{
    public class Exchange
    {
        public Book Book { get; set; }
        public decimal ExchangeFounds { get; set; }
        public Order BestOrder
        {
            get
            {
                return Book.Orders.FirstOrDefault();
            }
        }
        internal void RemoveBestOrder()
        {
            Book.Orders.RemoveAt(0);
        }
        internal decimal CalculatePossibleBuy(decimal amount)
        {
            decimal moneySpent;

            decimal amountLeft = (amount - BestOrder.Amount);
            moneySpent = (BestOrder.Amount + (amountLeft < 0 ? amountLeft : 0)) * BestOrder.Price;

            decimal foundsDifference = ExchangeFounds - moneySpent;
            if (foundsDifference < 0)
            {
                moneySpent += foundsDifference;
            }

            decimal amountBought = (moneySpent / BestOrder.Price);
            ExchangeFounds -= moneySpent;

            BestOrder.Amount = amountBought;

            return amount - amountBought;
        }

        internal decimal CalculatePossibleSell(decimal amount)
        {
            var amountBought = (amount - BestOrder.Amount) < 0 ? amount : (BestOrder.Amount);

            var foundsDifference = ExchangeFounds - amountBought;
            if (foundsDifference < 0)
            {
                amountBought += foundsDifference;
            }
            ExchangeFounds -= amountBought;
            BestOrder.Amount = amountBought;

            return amount - BestOrder.Amount;
        }
    }
}
