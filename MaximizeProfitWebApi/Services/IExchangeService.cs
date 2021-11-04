
using MaximizeProfitLib;

namespace MaximizeProfitWebApi.Services
{
    public interface IExchangeService
    {
        MetaExchange GetMetaExchange(string typeOfOrder);
    }
}
