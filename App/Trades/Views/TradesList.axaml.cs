using App.Lib;
using App.Trades.ViewModels;
using Avalonia.Controls;

namespace App.Trades.Views;

public partial class TradesList : UserControl
{
    public TradesList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<TradesListViewModel>();
    }
}
