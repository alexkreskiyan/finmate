using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Core.Mapper;
using Annium.Finance.Providers.Abstractions.Domain.Models;
using Annium.Logging;
using App.Lib;
using App.Main.Services;
using App.Trades.Models;
using Avalonia.ReactiveUI;

namespace App.Trades.ViewModels;

public class TradesListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    private readonly IMapper _mapper;
    public ILogger Logger { get; }
    public ObservableCollection<Trade> Items { get; }
    private readonly Comparison<Trade> _comparison = (a, b) => a.Moment < b.Moment ? 1 : -1;

    public TradesListViewModel(Link link, IMapper mapper, ILogger logger)
    {
        Logger = logger;
        _mapper = mapper;

        this.Trace("init trades");
        Items = new ObservableCollection<Trade>();

        this.Trace("observe trades");
        link.UserConnector.Trades.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleTrade);

        this.Trace("done");
    }

    private void HandleTrade(TradeModel model)
    {
        var item = Items.FirstOrDefault(t => t.OrderId == model.OrderId && t.Id == model.Id);

        if (item is not null)
        {
            this.Error("Trade {trade} already loaded", model);
            return;
        }

        Items.Add(_mapper.Map<Trade>(model));
        Items.ForceSort(_comparison);
    }
}
