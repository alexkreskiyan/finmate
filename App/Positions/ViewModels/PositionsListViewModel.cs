using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Data.Tables;
using Annium.Finance.Providers.Abstractions.Domain.Dto;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Positions.Models;
using Avalonia.ReactiveUI;

namespace App.Positions.ViewModels;

public class PositionsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Position> Positions { get; }

    public PositionsListViewModel(Link link, ILogger logger)
    {
        Logger = logger;

        this.Trace("init positions");
        Positions = new ObservableCollection<Position>();

        this.Trace("observe positions");
        link.UserConnector.Positions.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandlePosition);

        this.Trace("done");
    }

    private void HandlePosition(ChangeEvent<PositionDto> e)
    {
        switch (e.Type)
        {
            case ChangeEventType.Init:
                Positions.Clear();
                foreach (var x in e.Items)
                    SetPosition(x);
                break;
            case ChangeEventType.Set:
                SetPosition(e.Item);
                break;
            case ChangeEventType.Delete:
                var item = e.Item;
                var position = Positions.FirstOrDefault(
                    x => x.Symbol == item.Symbol && x.Orientation == item.OrientationRange
                );
                if (position is not null)
                    Positions.Remove(position);
                break;
        }
    }

    private void SetPosition(PositionDto x)
    {
        var position = Positions.FirstOrDefault(p => p.Symbol == x.Symbol && p.Orientation == x.OrientationRange);
        if (x.Amount == 0m)
        {
            if (position is not null)
                Positions.Remove(position);

            return;
        }

        if (position is not null)
        {
            position.MarginType = x.MarginType;
            position.Leverage = x.Leverage;
            position.Amount = x.Amount;
        }
        else
        {
            Positions.Add(new Position(x.Symbol, x.OrientationRange, x.MarginType, x.Leverage, x.Amount));
            Positions.Sort((a, b) => string.CompareOrdinal(a.Symbol, b.Symbol));
        }
    }
}
