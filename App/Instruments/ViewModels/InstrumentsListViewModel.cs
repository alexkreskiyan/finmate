using System;
using System.Collections.ObjectModel;
using System.Linq;
using Annium;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Lib;
using App.Main.Models;
using DynamicData;
using NodaTime;

namespace App.Instruments.ViewModels;

public class InstrumentsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<InstrumentTicker> Tickers { get; }

    public InstrumentsListViewModel(Configuration configuration, Main.Services.Connection connection, ILogger logger)
    {
        Logger = logger;

        this.Trace("init tickers");
        Tickers = new ObservableCollection<InstrumentTicker>(
            configuration.Symbols.OrderBy(x => x).Select(x => new InstrumentTicker(x, 0, 0))
        );

        this.Trace("observe tickers");
        connection.MarketConnector.Tickers
            .ThrottleBy(x => x.Symbol, Duration.FromMilliseconds(100))
            .Subscribe(HandleTicker);

        this.Trace("done");
    }

    private void HandleTicker(InstrumentTicker ticker)
    {
        this.Trace("handle {ticker}", ticker);
        var existing = Tickers.FirstOrDefault(x => x.Symbol == ticker.Symbol).NotNull();
        Tickers.Replace(existing, ticker);
    }
}
