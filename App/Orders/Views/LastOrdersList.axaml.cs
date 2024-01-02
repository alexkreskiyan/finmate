using App.Lib;
using App.Orders.ViewModels;
using Avalonia.Controls;

namespace App.Orders.Views;

public partial class LastOrdersList : UserControl
{
    public LastOrdersList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<LastOrdersListViewModel>();
    }
}
