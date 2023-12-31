using App.Assets.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Assets.Views;

public partial class AssetsList : UserControl
{
    public AssetsList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<AssetsListViewModel>();
    }
}
