using System;
using Annium.Core.DependencyInjection;
using Annium.Logging.File;
using Annium.Logging.Shared;
using App.Lib;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace App;

internal class ServicePack : ServicePackBase
{
    public override void Register(IServiceContainer container, IServiceProvider provider)
    {
        // core
        container.AddRuntime(GetType().Assembly);
        container.AddTime().WithRealTime().SetDefault();
        container.AddLogging();

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
        provider.UseLogging(
            route => route.UseFile(new FileLoggingConfiguration<DefaultLogContext> { GetFile = _ => "app.log" })
        );
    }
}
