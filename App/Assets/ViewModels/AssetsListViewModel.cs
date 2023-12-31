using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Annium.Collections.ObjectModel;
using Annium.Data.Tables;
using Annium.Finance.Providers.Abstractions.Domain.Dto;
using Annium.Logging;
using App.Assets.Models;
using App.Lib;
using App.Main.Services;
using Avalonia.ReactiveUI;

namespace App.Assets.ViewModels;

public class AssetsListViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }
    public ObservableCollection<Asset> Assets { get; }

    public AssetsListViewModel(Link link, ILogger logger)
    {
        Logger = logger;

        this.Trace("init assets");
        Assets = new ObservableCollection<Asset>();

        this.Trace("observe assets");
        link.UserConnector.Assets.SubscribeOn(AvaloniaScheduler.Instance).Subscribe(HandleAsset);

        this.Trace("done");
    }

    private void HandleAsset(ChangeEvent<AssetDto> e)
    {
        switch (e.Type)
        {
            case ChangeEventType.Init:
                Assets.Clear();
                foreach (var x in e.Items)
                    SetAsset(x);
                break;
            case ChangeEventType.Set:
                SetAsset(e.Item);
                break;
            case ChangeEventType.Delete:
                var item = e.Item;
                var asset = Assets.FirstOrDefault(a => a.Resource == item.Resource);
                if (asset is not null)
                {
                    Assets.Remove(asset);
                }
                break;
        }
    }

    private void SetAsset(AssetDto x)
    {
        var asset = Assets.FirstOrDefault(a => a.Resource == x.Resource);
        if (asset is not null)
        {
            asset.Free = x.Free;
            asset.Locked = x.Locked;
        }
        else
        {
            Assets.Add(new Asset(x.Resource, x.Free, x.Locked));
            Assets.Sort((a, b) => string.CompareOrdinal(a.Resource, b.Resource));
        }
    }
}
