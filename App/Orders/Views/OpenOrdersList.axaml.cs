using App.Lib;
using App.Orders.ViewModels;
using Avalonia.Controls;

namespace App.Orders.Views;

public partial class OpenOrdersList : UserControl
{
    public OpenOrdersList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<OpenOrdersListViewModel>();
    }
}
