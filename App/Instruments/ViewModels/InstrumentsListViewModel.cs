using System;
using System.Collections.ObjectModel;
using System.Linq;
using Annium;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Instruments.Models;
using App.Lib;
using App.Main.Models;
using App.Main.Services;
using Avalonia.Controls;
using NodaTime;

namespace App.Instruments.ViewModels;

public class InstrumentsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Ticker> Tickers { get; }
    private readonly Link _link;

    public InstrumentsListViewModel(Configuration configuration, Link link, ILogger logger)
    {
        _link = link;
        Logger = logger;

        this.Trace("init tickers");
        Tickers = new ObservableCollection<Ticker>(
            configuration.Symbols.OrderBy(x => x).Select(x => new Ticker(x, 0, 0))
        );

        this.Trace("observe tickers");
        link.MarketConnector.Tickers.ThrottleBy(x => x.Symbol, Duration.FromMilliseconds(100)).Subscribe(HandleTicker);

        this.Trace("done");
    }

    public void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 0)
            return;

        var ticker = e.AddedItems[0].NotNull().CastTo<Ticker>();
        _link.Symbol = ticker.Symbol;
        _link.Price = (ticker.BidPrice + ticker.AskPrice) / 2;
    }

    private void HandleTicker(InstrumentTicker update)
    {
        var ticker = Tickers.FirstOrDefault(x => x.Symbol == update.Symbol).NotNull();
        ticker.BidPrice = update.BidPrice;
        ticker.AskPrice = update.AskPrice;

        if (ticker.Symbol == _link.Symbol)
            _link.Price = (ticker.BidPrice + ticker.AskPrice) / 2;
    }
}
