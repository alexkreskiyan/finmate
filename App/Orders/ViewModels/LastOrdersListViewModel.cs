using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Core.Mapper;
using Annium.Data.Tables;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Orders.Models;
using Avalonia.ReactiveUI;
using NodaTime;

namespace App.Orders.ViewModels;

public class LastOrdersListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    private const int MaxOrders = 3;
    public ILogger Logger { get; }
    public ObservableCollection<Order> Items { get; }
    private readonly IMapper _mapper;
    private readonly Comparison<Order> _comparison = (a, b) => a.CreatedAt < b.CreatedAt ? 1 : -1;

    public LastOrdersListViewModel(Link link, IMapper mapper, ILogger logger)
    {
        _mapper = mapper;
        Logger = logger;

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
                SetItem(e.Item);
                break;
        }
    }

    private void InitItems(IReadOnlyCollection<OrderModel> models)
    {
        foreach (var model in models)
        {
            var item = Items.FirstOrDefault(x => x.Id == model.Id);

            if (item is null)
            {
                Items.Add(_mapper.Map<Order>(model));
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

        Items.ForceSort(_comparison);

        while (Items.Count > MaxOrders)
            Items.Remove(Items[^1]);
    }

    private void SetItem(OrderModel model)
    {
        var item = Items.FirstOrDefault(x => x.Id == model.Id);

        if (item is null)
        {
            Items.Add(_mapper.Map<Order>(model));
            Items.ForceSort(_comparison);

            while (Items.Count > MaxOrders)
                Items.Remove(Items[^1]);
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
}
