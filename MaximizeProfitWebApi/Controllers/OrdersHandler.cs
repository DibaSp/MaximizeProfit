using MaximizeProfitLib;
using MaximizeProfitLib.Models;
using MaximizeProfitWebApi.Dto;
using MaximizeProfitWebApi.Services;
using System.Collections.Generic;

namespace MaximizeProfitWebApi.Controllers
{
    public class OrdersHandler : IOrdersHandler
    {
        private readonly IExchangeService exchangeService;

        public OrdersHandler(IExchangeService exchangeService)
        {
            this.exchangeService = exchangeService;
        }
        public GetOrdersDtoResult GetOptimalOrders(GetOrdersDto getOrdersBody)
        {
            MetaExchange metaExchange = exchangeService.GetMetaExchange(getOrdersBody.Type);
            List<Order> orders;
            if(getOrdersBody.Type.ToLower().Trim().Trim().Equals("buy"))
            {
                orders = metaExchange.GetBuyOrdersForAmount(getOrdersBody.Amount);

            } else
            {
                orders = metaExchange.GetSellOrdersForAmount(getOrdersBody.Amount);

            }

            return new GetOrdersDtoResult
            {
                Orders = orders
            };
        }
    }
}
