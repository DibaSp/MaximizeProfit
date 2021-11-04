using MaximizeProfitWebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MaximizeProfitWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersHandler ordersHandler;

        public OrdersController(IOrdersHandler ordersHandler)
        {
            this.ordersHandler = ordersHandler;
        }

        [HttpPost]
        public IActionResult GetBestExecutionPlan(GetOrdersDto getOrdersBody)
        {
            GetOrdersDtoResult getOrders = ordersHandler.GetOptimalOrders(getOrdersBody);

            return Created("", getOrders);
        }
    }
}
