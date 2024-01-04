using App.Lib;
using App.Trading.ViewModels;
using Avalonia.Controls;

namespace App.Trading.Views;

public partial class LevelMarketOrderPanel : UserControl
{
    public LevelMarketOrderPanel()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<LevelMarketOrderPanelViewModel>();
    }
}
