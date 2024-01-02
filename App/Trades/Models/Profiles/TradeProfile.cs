using Annium.Core.Mapper;
using Annium.Finance.Providers.Abstractions.Domain.Models;

namespace App.Trades.Models.Profiles;

public class TradeProfile : Profile
{
    public TradeProfile()
    {
        Map<TradeModel, Trade>(
            x =>
                new Trade(
                    x.Id,
                    x.OrderId,
                    x.Symbol,
                    x.Price,
                    x.Qty,
                    x.CommissionAsset,
                    x.CommissionAmount,
                    x.Maker,
                    x.Moment
                )
        );
    }
}
