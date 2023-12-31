using App.Candles.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Candles.Views;

public partial class CandlesList : UserControl
{
    public CandlesList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<CandlesListViewModel>();
    }
}
