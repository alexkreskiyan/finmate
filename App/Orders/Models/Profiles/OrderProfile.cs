using Annium.Core.Mapper;
using Annium.Finance.Providers.Abstractions.Domain.Models;

namespace App.Orders.Models.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        Map<OrderModel, Order>(x => new Order(
            x.Id,
            x.ClientOrderId,
            x.Symbol,
            x.Side,
            x.Type,
            x.TotalQty,
            x.Price,
            x.LevelPrice,
            x.ReduceOnly,
            x.CreatedAt,
            x.Status,
            x.ExecutedQty,
            x.ExecutedPrice,
            x.UpdatedAt
        ));
    }
}
