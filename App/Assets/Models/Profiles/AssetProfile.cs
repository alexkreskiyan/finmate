using Annium.Core.Mapper;
using Annium.Finance.Providers.Abstractions.Domain.Models;

namespace App.Assets.Models.Profiles;

public class AssetProfile : Profile
{
    public AssetProfile()
    {
        Map<AssetModel, Asset>(x => new Asset(x.Resource, x.Free, x.Locked));
    }
}
