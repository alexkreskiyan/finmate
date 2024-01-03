using App.Lib;
using App.Trading.ViewModels;
using Avalonia.Controls;

namespace App.Trading.Views;

public partial class LimitOrderPanel : UserControl
{
    public LimitOrderPanel()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<LimitOrderPanelViewModel>();
    }
}
