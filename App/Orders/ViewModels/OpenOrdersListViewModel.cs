using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Data.Tables;
using Annium.Finance.Providers.Abstractions.Domain.Enums;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Orders.Models;
using Avalonia.ReactiveUI;
using NodaTime;

namespace App.Orders.ViewModels;

public class OpenOrdersListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Order> Orders { get; }

    public OpenOrdersListViewModel(Link link, ILogger logger)
    {
        Logger = logger;

        this.Trace("init orders");
        Orders = new ObservableCollection<Order>();

        this.Trace("observe orders");
        link.UserConnector.Orders.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleOrder);

        this.Trace("done");
    }

    private void HandleOrder(ChangeEvent<OrderModel> e)
    {
        switch (e.Type)
        {
            case ChangeEventType.Init:
                Orders.Clear();
                foreach (var x in e.Items)
                    SetOrder(x);
                break;
            case ChangeEventType.Set:
                SetOrder(e.Item);
                break;
            case ChangeEventType.Delete:
                var item = e.Item;
                var order = Orders.FirstOrDefault(x => x.Id == item.Id);
                if (order is not null)
                    Orders.Remove(order);
                break;
        }
    }

    private void SetOrder(OrderModel x)
    {
        var order = Orders.FirstOrDefault(p => p.Id == x.Id);

        // remove order in terminal status
        if (x.Status is OrderStatus.Filled or OrderStatus.Expired or OrderStatus.Canceled)
        {
            if (order is not null)
                Orders.Remove(order);

            return;
        }

        if (order is not null)
        {
            order.Status = x.Status;
            order.ExecutedQty = x.ExecutedQty;
            order.ExecutedPrice = x.ExecutedPrice;
            order.UpdatedAt = Instant.FromUnixTimeMilliseconds(x.UpdatedAt).LocalDateTime();
        }
        else
        {
            Orders.Add(
                new Order(
                    x.Id,
                    x.ClientOrderId,
                    x.Symbol,
                    x.Side,
                    x.Type,
                    x.TotalQty,
                    x.Price,
                    x.LevelPrice,
                    x.ReduceOnly,
                    x.CreatedAt,
                    x.Status,
                    x.ExecutedQty,
                    x.ExecutedPrice,
                    x.UpdatedAt
                )
            );
            Orders.Sort((a, b) => -1 * string.CompareOrdinal(a.UpdatedAt, b.UpdatedAt));
        }
    }
}
