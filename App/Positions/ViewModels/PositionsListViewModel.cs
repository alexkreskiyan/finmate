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
using App.Positions.Models;
using Avalonia.ReactiveUI;
using DynamicData;

namespace App.Positions.ViewModels;

public class PositionsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Position> Items { get; }
    private readonly IMapper _mapper;
    private readonly Comparison<Position> _comparison = (a, b) => string.CompareOrdinal(a.Symbol, b.Symbol);

    public PositionsListViewModel(Link link, IMapper mapper, ILogger logger)
    {
        _mapper = mapper;
        Logger = logger;

        this.Trace("init positions");
        Items = new ObservableCollection<Position>();

        this.Trace("observe positions");
        link.UserConnector.Positions.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandlePosition);

        this.Trace("done");
    }

    private void HandlePosition(ChangeEvent<PositionModel> e)
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

    private void InitItems(IReadOnlyCollection<PositionModel> models)
    {
        Items.Clear();
        var items = models.Where(x => x.Amount != 0).Select(_mapper.Map<Position>).ToList();
        items.Sort(_comparison);
        Items.AddRange(items);
    }

    private void SetItem(PositionModel model)
    {
        var item = Items.FirstOrDefault(x => x.Symbol == model.Symbol && x.Orientation == model.OrientationRange);

        if (model.Amount == 0m)
        {
            if (item is not null)
                Items.Remove(item);

            return;
        }

        if (item is null)
        {
            Items.Add(_mapper.Map<Position>(model));
            Items.ForceSort(_comparison);
        }
        else
        {
            item.MarginType = model.MarginType;
            item.Leverage = model.Leverage;
            item.Amount = model.Amount;
        }
    }

    private void DeleteItem(PositionModel model)
    {
        var item = Items.FirstOrDefault(x => x.Symbol == model.Symbol && x.Orientation == model.OrientationRange);
        if (item is not null)
            Items.Remove(item);
    }
}
