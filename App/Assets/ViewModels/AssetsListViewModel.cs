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
using App.Assets.Models;
using App.Lib;
using App.Main.Services;
using Avalonia.ReactiveUI;
using DynamicData;

namespace App.Assets.ViewModels;

public class AssetsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Asset> Items { get; }
    private readonly IMapper _mapper;
    private readonly Comparison<Asset> _comparison = (a, b) => string.CompareOrdinal(a.Resource, b.Resource);

    public AssetsListViewModel(Link link, IMapper mapper, ILogger logger)
    {
        Logger = logger;
        _mapper = mapper;

        this.Trace("init assets");
        Items = new ObservableCollection<Asset>();

        this.Trace("observe assets");
        link.UserConnector.Assets.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleAsset);

        this.Trace("done");
    }

    private void HandleAsset(ChangeEvent<AssetModel> e)
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

    private void InitItems(IReadOnlyCollection<AssetModel> models)
    {
        Items.Clear();
        var assets = models.Where(x => x.Free != 0 || x.Locked != 0).Select(_mapper.Map<Asset>).ToList();
        assets.Sort(_comparison);
        Items.AddRange(assets);
    }

    private void SetItem(AssetModel model)
    {
        var item = Items.FirstOrDefault(a => a.Resource == model.Resource);

        if (model.Free == 0m && model.Locked == 0m)
        {
            if (item is not null)
                Items.Remove(item);

            return;
        }

        if (item is null)
        {
            Items.Add(_mapper.Map<Asset>(model));
            Items.ForceSort(_comparison);
        }
        else
        {
            item.Free = model.Free;
            item.Locked = model.Locked;
        }
    }

    private void DeleteItem(AssetModel model)
    {
        var item = Items.FirstOrDefault(a => a.Resource == model.Resource);
        if (item is not null)
            Items.Remove(item);
    }
}
