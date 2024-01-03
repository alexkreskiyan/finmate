using Annium.Finance.Providers.Abstractions.Domain.Enums;
using App.Lib;
using NodaTime;
using ReactiveUI;

namespace App.Orders.Models;

public sealed record Order : ReactiveRecord
{
    public string Id { get; init; }
    public string ClientOrderId { get; init; }
    public string Symbol { get; init; }
    public OrderSide Side { get; init; }
    public OrderType Type { get; init; }
    public decimal TotalQty { get; init; }
    public decimal Price { get; init; }
    public decimal LevelPrice { get; init; }
    public bool ReduceOnly { get; init; }
    public long CreatedAt { get; init; }
    public string CreatedAtString { get; init; }

    public OrderStatus Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public decimal ExecutedQty
    {
        get => _executedQty;
        set => this.RaiseAndSetIfChanged(ref _executedQty, value);
    }

    public decimal ExecutedPrice
    {
        get => _executedPrice;
        set => this.RaiseAndSetIfChanged(ref _executedPrice, value);
    }

    public long UpdatedAt
    {
        get => _updatedAt;
        set => this.RaiseAndSetIfChanged(ref _updatedAt, value);
    }

    public string UpdatedAtString
    {
        get => _updatedAtString;
        set => this.RaiseAndSetIfChanged(ref _updatedAtString, value);
    }

    private OrderStatus _status;
    private decimal _executedQty;
    private decimal _executedPrice;
    private long _updatedAt;
    private string _updatedAtString;

    public Order(
        string id,
        string clientOrderId,
        string symbol,
        OrderSide side,
        OrderType type,
        decimal totalQty,
        decimal price,
        decimal levelPrice,
        bool reduceOnly,
        long createdAt,
        OrderStatus status,
        decimal executedQty,
        decimal executedPrice,
        long updatedAt
    )
    {
        Id = id;
        ClientOrderId = clientOrderId;
        Symbol = symbol;
        Side = side;
        Type = type;
        TotalQty = totalQty;
        Price = price;
        LevelPrice = levelPrice;
        ReduceOnly = reduceOnly;
        CreatedAt = createdAt;
        CreatedAtString = Instant.FromUnixTimeMilliseconds(createdAt).LocalDateTime();
        _status = status;
        _executedQty = executedQty;
        _executedPrice = executedPrice;
        _updatedAt = updatedAt;
        _updatedAtString = Instant.FromUnixTimeMilliseconds(updatedAt).LocalDateTime();
    }
}
