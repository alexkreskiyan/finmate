using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Trades.Models;
using Avalonia.ReactiveUI;

namespace App.Trades.ViewModels;

public class TradesListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Trade> Trades { get; }

    public TradesListViewModel(Link link, ILogger logger)
    {
        Logger = logger;

        this.Trace("init positions");
        Trades = new ObservableCollection<Trade>();

        this.Trace("observe positions");
        link.UserConnector.Trades.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleTrade);

        this.Trace("done");
    }

    private void HandleTrade(TradeModel x)
    {
        var trade = Trades.FirstOrDefault(t => t.OrderId == x.OrderId && t.Id == x.Id);

        if (trade is not null)
        {
            this.Error("Trade {trade} already loaded", x);
            return;
        }

        Trades.Add(
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
        Trades.Sort((a, b) => string.CompareOrdinal(a.Moment, b.Moment));
    }
}
