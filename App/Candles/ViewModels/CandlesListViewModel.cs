using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Annium;
using Annium.Logging;
using Annium.NodaTime.Extensions;
using Annium.Threading;
using App.Candles.Models;
using App.Lib;
using App.Main.Models;
using App.Main.Services;
using Avalonia.Threading;
using DynamicData.Binding;
using NodaTime;

namespace App.Candles.ViewModels;

public class CandlesListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Candle> Candles { get; }
    private readonly Configuration _config;
    private readonly ITimeProvider _timeProvider;
    private readonly Link _link;

    public CandlesListViewModel(Configuration config, ITimeProvider timeProvider, Link link, ILogger logger)
    {
        _config = config;
        _timeProvider = timeProvider;
        _link = link;
        Logger = logger;

        this.Trace("init tickers");
        Candles = new ObservableCollection<Candle>();

        this.Trace("init timer");
        var timer = Timers.Async(LoadCandles, 0, 2000);

        this.Trace("handle symbol change");
        link.WhenPropertyChanged(x => x.Symbol).Subscribe(_ => timer.Change(0, 2000));

        this.Trace("done");
    }

    private async ValueTask LoadCandles()
    {
        var symbol = _link.Symbol;
        var env = _config.Settings.Environment;
        var end = _timeProvider.Now.CeilToMinute();
        var start = end.Minus(Duration.FromMinutes(10));

        this.Trace("load {env} {symbol} in {start} - {end}", env, symbol, start, end);

        await foreach (var result in _link.MarketProvider.LoadCandlesAsync(symbol, env, start, end, default))
        {
            if (result.IsFailure)
            {
                this.Error("Load failed: {result}", result);
                continue;
            }

            Dispatcher.UIThread.Post(() =>
            {
                Candles.Clear();
                foreach (var x in result.Data)
                    Candles.Insert(0, new Candle(x.Moment, x.Open, x.High, x.Low, x.Close, x.Volume));
            });
        }
    }
}