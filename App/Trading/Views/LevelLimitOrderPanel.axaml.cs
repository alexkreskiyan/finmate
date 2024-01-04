using App.Lib;
using App.Trading.ViewModels;
using Avalonia.Controls;

namespace App.Trading.Views;

public partial class LevelLimitOrderPanel : UserControl
{
    public LevelLimitOrderPanel()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<LevelLimitOrderPanelViewModel>();
    }
}
