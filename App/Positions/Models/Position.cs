using Annium.Finance.Providers.Abstractions.Domain.Enums;
using ReactiveUI;

namespace App.Positions.Models;

public sealed record Position : ReactiveRecord
{
    public string Symbol { get; }
    public OrientationRange Orientation { get; }

    public MarginType MarginType
    {
        get => _marginType;
        set => this.RaiseAndSetIfChanged(ref _marginType, value);
    }

    public decimal Leverage
    {
        get => _leverage;
        set => this.RaiseAndSetIfChanged(ref _leverage, value);
    }

    public decimal Amount
    {
        get => _amount;
        set => this.RaiseAndSetIfChanged(ref _amount, value);
    }

    private MarginType _marginType;
    private decimal _leverage;
    private decimal _amount;

    public Position(
        string symbol,
        OrientationRange orientation,
        MarginType marginType,
        decimal leverage,
        decimal amount
    )
    {
        Symbol = symbol;
        Orientation = orientation;
        _marginType = marginType;
        _leverage = leverage;
        _amount = amount;
    }
}
