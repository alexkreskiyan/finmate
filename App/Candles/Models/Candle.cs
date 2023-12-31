using Annium.NodaTime.Extensions;
using NodaTime;
using ReactiveUI;

namespace App.Candles.Models;

public sealed record Candle : ReactiveRecord
{
    public string Moment { get; }
    public decimal Open
    {
        get => _open;
        set => this.RaiseAndSetIfChanged(ref _open, value);
    }
    public decimal High
    {
        get => _high;
        set => this.RaiseAndSetIfChanged(ref _high, value);
    }
    public decimal Low
    {
        get => _low;
        set => this.RaiseAndSetIfChanged(ref _low, value);
    }
    public decimal Close
    {
        get => _close;
        set => this.RaiseAndSetIfChanged(ref _close, value);
    }
    public decimal Volume
    {
        get => _volume;
        set => this.RaiseAndSetIfChanged(ref _volume, value);
    }

    private decimal _open;
    private decimal _high;
    private decimal _low;
    private decimal _close;
    private decimal _volume;

    public Candle(long moment, decimal open, decimal high, decimal low, decimal close, decimal volume)
    {
        Moment = Instant.FromUnixTimeMilliseconds(moment).InLocal().ToString("HH:mm:ss", null);
        Open = open;
        High = high;
        Low = low;
        Close = close;
        Volume = volume;
    }
}
