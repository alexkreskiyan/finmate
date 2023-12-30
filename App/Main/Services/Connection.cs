using System;
using Annium;
using Annium.Core.Mapper;
using Annium.Extensions.Pooling;
using Annium.Finance.Providers.Abstractions.Connectors.Connectors;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using App.Lib;
using App.Main.Models;

namespace App.Main.Services;

public class Connection : ISingleton
{
    public IMarketConnector MarketConnector { get; }
    public IUserConnector UserConnector { get; }

    public Connection(
        Configuration config,
        IObjectCache<MarketSettings, IMarketConnector> marketCache,
        IObjectCache<UserSettings, IUserConnector> userCache,
        IMapper mapper
    )
    {
        config.Settings.NotNull();
        if (config.Symbols.Length == 0)
            throw new InvalidOperationException("No instruments configured");

        MarketConnector = marketCache.GetAsync(mapper.Map<MarketSettings>(config.Settings)).Result.Value;
        UserConnector = userCache.GetAsync(config.Settings).Result.Value;
    }
}
