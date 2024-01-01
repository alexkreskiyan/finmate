using Annium.NodaTime.Extensions;
using NodaTime;

namespace App.Trades.Models;

public sealed record Trade
{
    public string Id { get; init; }
    public string OrderId { get; init; }
    public string Symbol { get; init; }
    public decimal Price { get; init; }
    public decimal Qty { get; init; }
    public string CommissionAsset { get; init; }
    public decimal CommissionAmount { get; init; }
    public bool Maker { get; init; }
    public string Moment { get; init; }

    public Trade(
        string id,
        string orderId,
        string symbol,
        decimal price,
        decimal qty,
        string commissionAsset,
        decimal commissionAmount,
        bool maker,
        long moment
    )
    {
        Id = id;
        OrderId = orderId;
        Symbol = symbol;
        Price = price;
        Qty = qty;
        CommissionAsset = commissionAsset;
        CommissionAmount = commissionAmount;
        Maker = maker;
        Moment = Instant.FromUnixTimeMilliseconds(moment).InLocal().ToString("dd.MM.yy HH:mm:ss", null);
    }
}
