using ReactiveUI;

namespace App.Instruments.Models;

public sealed record Ticker : ReactiveRecord
{
    public string Symbol { get; }
    public decimal BidPrice
    {
        get => _bidPrice;
        set => this.RaiseAndSetIfChanged(ref _bidPrice, value);
    }
    public decimal AskPrice
    {
        get => _askPrice;
        set => this.RaiseAndSetIfChanged(ref _askPrice, value);
    }

    private decimal _bidPrice;
    private decimal _askPrice;

    public Ticker(string symbol, decimal bidPrice, decimal askPrice)
    {
        Symbol = symbol;
        _bidPrice = bidPrice;
        _askPrice = askPrice;
    }
}
