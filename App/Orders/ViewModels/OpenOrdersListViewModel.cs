using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Annium.Collections.ObjectModel;
using Annium.Core.Mapper;
using Annium.Data.Tables;
using Annium.Finance.Providers.Abstractions.Domain.Enums;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Finance.Providers.Abstractions.Domain.Tools;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Orders.Models;
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using NodaTime;

namespace App.Orders.ViewModels;

public partial class OpenOrdersListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Order> Items { get; }
    private readonly Link _link;
    private readonly IMapper _mapper;
    private readonly Comparison<Order> _comparison = (a, b) => a.CreatedAt < b.CreatedAt ? 1 : -1;

    public OpenOrdersListViewModel(Link link, IMapper mapper, ILogger logger)
    {
        Logger = logger;
        _link = link;
        _mapper = mapper;

        this.Trace("init orders");
        Items = new ObservableCollection<Order>();

        this.Trace("observe orders");
        link.UserConnector.Orders.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleOrder);

        this.Trace("done");
    }

    private void HandleOrder(ChangeEvent<OrderModel> e)
    {
        switch (e.Type)
        {
            case ChangeEventType.Init:
                InitItems(e.Items);
                break;
            case ChangeEventType.Set:
                SetItem(e.Item);
                break;
            case ChangeEventType.Delete:
                DeleteItem(e.Item);
                break;
        }
    }

    private void InitItems(IReadOnlyCollection<OrderModel> models)
    {
        Items.Clear();
        var items = models
            .Where(x => x.Status is not (OrderStatus.Filled or OrderStatus.Expired or OrderStatus.Canceled))
            .Select(_mapper.Map<Order>)
            .ToList();
        items.Sort(_comparison);
        Items.AddRange(items);
    }

    private void SetItem(OrderModel model)
    {
        var item = Items.FirstOrDefault(x => x.Id == model.Id);

        if (model.Status is OrderStatus.Filled or OrderStatus.Expired or OrderStatus.Canceled)
        {
            if (item is not null)
                Items.Remove(item);

            return;
        }

        if (item is null)
        {
            Items.Add(_mapper.Map<Order>(model));
            Items.ForceSort(_comparison);
        }
        else
        {
            item.Status = model.Status;
            item.ExecutedQty = model.ExecutedQty;
            item.ExecutedPrice = model.ExecutedPrice;
            item.UpdatedAt = model.UpdatedAt;
            item.UpdatedAtString = Instant.FromUnixTimeMilliseconds(model.UpdatedAt).LocalDateTime();
        }
    }

    private void DeleteItem(OrderModel model)
    {
        var item = Items.FirstOrDefault(x => x.Id == model.Id);
        if (item is not null)
            Items.Remove(item);
    }

    [RelayCommand]
    private async Task CancelOrder(Order? x)
    {
        if (x is null)
            return;

        var request = RequestBuilder.CancelOrder(x.Id, x.ClientOrderId, x.Symbol);

        await _link.UserConnector.CancelOrder(request);
    }
}
