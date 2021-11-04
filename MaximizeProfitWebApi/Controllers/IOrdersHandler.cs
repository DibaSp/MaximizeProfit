
using MaximizeProfitWebApi.Dto;

namespace MaximizeProfitWebApi.Controllers
{
    public interface IOrdersHandler
    {
        GetOrdersDtoResult GetOptimalOrders(GetOrdersDto getOrdersBody);
    }
}
