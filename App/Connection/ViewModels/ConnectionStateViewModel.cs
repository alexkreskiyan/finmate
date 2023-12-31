using System;
using Annium.Finance.Providers.Abstractions.Connectors.Connectors;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using Avalonia.Media;
using ReactiveUI;

namespace App.Connection.ViewModels;

public class ConnectionStateViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }

    public IBrush MarketColor
    {
        get => _marketColor;
        set => this.RaiseAndSetIfChanged(ref _marketColor, value);
    }

    public IBrush UserColor
    {
        get => _userColor;
        set => this.RaiseAndSetIfChanged(ref _userColor, value);
    }

    private IBrush _marketColor = GetConnectorStatusColor(ConnectorStatus.Disconnected);
    private IBrush _userColor = GetConnectorStatusColor(ConnectorStatus.Disconnected);

    public ConnectionStateViewModel(Link link, ILogger logger)
    {
        Logger = logger;

        this.Trace("setup market color tracking");
        link.MarketConnector.OnStatusChanged += status => MarketColor = GetConnectorStatusColor(status);

        this.Trace("setup market color to {status}", link.MarketConnector.Status);
        MarketColor = GetConnectorStatusColor(link.MarketConnector.Status);

        this.Trace("setup user color tracking");
        link.UserConnector.OnStatusChanged += status => UserColor = GetConnectorStatusColor(status);

        this.Trace("setup user color to {status}", link.UserConnector.Status);
        UserColor = GetConnectorStatusColor(link.UserConnector.Status);

        this.Trace("done");
    }

    private static IBrush GetConnectorStatusColor(ConnectorStatus status)
    {
        Console.WriteLine($"STATUS!: {status}");

        return status switch
        {
            ConnectorStatus.Connected => Brushes.Green,
            ConnectorStatus.Connecting => Brushes.Orange,
            _ => Brushes.Red,
        };
    }
}
