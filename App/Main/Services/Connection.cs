using System;
using Annium;
using Annium.Core.Mapper;
using Annium.Extensions.Pooling;
using Annium.Finance.Providers.Abstractions.Connectors.Connectors;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Finance.Providers.Crypto.Binance.UsdFutures;
using Annium.Logging;
using App.Lib;
using App.Main.Models;
using Microsoft.Extensions.DependencyInjection;

namespace App.Main.Services;

public class Connection : ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public IMarketProvider MarketProvider { get; }
    public IMarketConnector MarketConnector => _marketConnector.Value;
    public IUserProvider UserProvider { get; }
    public IUserConnector UserConnector => _userConnector.Value;
    private readonly Lazy<IMarketConnector> _marketConnector;
    private readonly Lazy<IUserConnector> _userConnector;
    private readonly Configuration _config;
    private readonly IObjectCache<MarketSettings, IMarketConnector> _marketCache;
    private readonly IObjectCache<UserSettings, IUserConnector> _userCache;
    private readonly IMapper _mapper;

    public Connection(
        Configuration config,
        [FromKeyedServices(Constants.Provider)] IMarketProvider marketProvider,
        [FromKeyedServices(Constants.Provider)] IUserProvider userProvider,
        IObjectCache<MarketSettings, IMarketConnector> marketCache,
        IObjectCache<UserSettings, IUserConnector> userCache,
        IMapper mapper,
        ILogger logger
    )
    {
        Logger = logger;
        config.Settings.NotNull();
        if (config.Symbols.Length == 0)
            throw new InvalidOperationException("No instruments configured");

        _config = config;
        _marketCache = marketCache;
        _userCache = userCache;
        _mapper = mapper;

        MarketProvider = marketProvider;
        UserProvider = userProvider;
        _marketConnector = new Lazy<IMarketConnector>(GetMarketConnector);
        _userConnector = new Lazy<IUserConnector>(GetUserConnector);
    }

    private IMarketConnector GetMarketConnector()
    {
        this.Trace("start");

        var connector = _marketCache.GetAsync(_mapper.Map<MarketSettings>(_config.Settings)).Result.Value;

        this.Trace("done");

        return connector;
    }

    private IUserConnector GetUserConnector()
    {
        this.Trace("start");

        var connector = _userCache.GetAsync(_config.Settings).Result.Value;

        this.Trace("done");

        return connector;
    }
}
