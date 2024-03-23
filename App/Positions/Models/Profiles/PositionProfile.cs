using Annium.Core.Mapper;
using Annium.Finance.Providers.Abstractions.Domain.Models;

namespace App.Positions.Models.Profiles;

public class PositionProfile : Profile
{
    public PositionProfile()
    {
        Map<PositionModel, Position>(x => new Position(
            x.Symbol,
            x.OrientationRange,
            x.MarginType,
            x.Leverage,
            x.Amount
        ));
    }
}
