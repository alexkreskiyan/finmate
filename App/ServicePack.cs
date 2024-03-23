using System;
using System.Collections.Immutable;
using System.Linq;
using Annium.Configuration.Abstractions;
using Annium.Core.DependencyInjection;
using Annium.Finance.Providers.Crypto.Binance.UsdFutures;
using App.Lib;
using App.Main.Models;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using LogLevel = Annium.Logging.LogLevel;

namespace App;

internal class ServicePack : ServicePackBase
{
    public override void Configure(IServiceContainer container)
    {
        container.AddConfiguration<Configuration>(x => x.AddYamlFile("config.yml"));
    }

    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // core
        container.AddRuntime(GetType().Assembly);
        container.AddTime().WithRealTime().SetDefault();
        container.AddLogging();
        container.AddMapper();

        // finance
        container.AddProviders().WithBinanceUsdFutures();

        // elements
        container.AddAll().AssignableTo<ISingleton>().AsSelf().Singleton();
        container.AddAll().AssignableTo<IScoped>().AsSelf().Scoped();
        container.AddAll().AssignableTo<ITransient>().AsSelf().Transient();

        // configure splat
        container.Collection.UseMicrosoftDependencyResolver();
        var resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();
    }

    public override void Setup(IServiceProvider provider)
    {
        var excludedSubjects = ImmutableArray.Create(
            "DataConnection",
            "ClientWebSocket",
            "ServerWebSocket",
            "ManagedWebSocket",
            "StatusMonitor",
            "StatusReporter",
            "ConnectionMonitor",
            "ParallelExecutor",
            "ConcurrentExecutor",
            "SequentialExecutor",
            "DisposableBox",
            "HttpRequest",
            "ObjectCache",
            "ServerTimeProvider",
            "MarketProvider",
            "UserProvider",
            "CompositeLoader",
            "SnapshotLoader",
            "KeyedLoader",
            "ListenKeyResolver"
        );
        provider.UseLogging(route =>
            route
                .For(x =>
                    x.Level >= LogLevel.Warn
                    || x.Level >= LogLevel.Trace && !excludedSubjects.Any(s => x.SubjectType.Contains(s))
                )
                .UseConsole()
        );
    }
}
