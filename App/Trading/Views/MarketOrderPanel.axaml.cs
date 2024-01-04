using App.Lib;
using App.Trading.ViewModels;
using Avalonia.Controls;

namespace App.Trading.Views;

public partial class MarketOrderPanel : UserControl
{
    public MarketOrderPanel()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<MarketOrderPanelViewModel>();
    }
}
