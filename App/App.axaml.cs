using System;
using Annium.Core.DependencyInjection;
using App.Lib;
using App.Main.ViewModels;
using App.Main.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Hosting;

namespace App;

public partial class App : Application
{
    public override void Initialize()
    {
        Environment.SetEnvironmentVariable("ANNIUM_LOG", "trace");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Provider.Init(Host.CreateDefaultBuilder().UseServicePack<ServicePack>().Build().Services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow { DataContext = Provider.Resolve<MainWindowViewModel>() };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
