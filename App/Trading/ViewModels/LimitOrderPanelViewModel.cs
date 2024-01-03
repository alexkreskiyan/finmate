using System;
using System.Threading.Tasks;
using Annium.Finance.Providers.Abstractions.Domain.Enums;
using Annium.Finance.Providers.Abstractions.Domain.Tools;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace App.Trading.ViewModels;

public partial class LimitOrderPanelViewModel : ViewModelBase, ISingleton, ILogSubject
{
    private readonly Link _link;
    public ILogger Logger { get; }
    public decimal? Qty
    {
        get => _qty;
        set => this.RaiseAndSetIfChanged(ref _qty, value);
    }
    public decimal? Price
    {
        get => _price;
        set => this.RaiseAndSetIfChanged(ref _price, value);
    }
    public bool ReduceOnly
    {
        get => _reduceOnly;
        set => this.RaiseAndSetIfChanged(ref _reduceOnly, value);
    }

    private decimal? _qty;
    private decimal? _price;
    private bool _reduceOnly;

    public LimitOrderPanelViewModel(Link link, ILogger logger)
    {
        _link = link;
        Logger = logger;
    }

    [RelayCommand]
    private Task Buy() => Init(OrderSide.Buy);

    [RelayCommand]
    private Task Sell() => Init(OrderSide.Sell);

    private async Task Init(OrderSide side)
    {
        if (Qty is null || Price is null)
        {
            this.Warn("qty: {qty}, price: {price}", Qty, Price);
            return;
        }

        var request = RequestBuilder.InitLimitOrder(
            Guid.NewGuid().ToString(),
            _link.Symbol,
            side,
            Qty.Value,
            Price.Value,
            ReduceOnly
        );
        var result = await _link.UserConnector.InitOrder(request);
        this.Info("Result: {result}", result);
    }
}
