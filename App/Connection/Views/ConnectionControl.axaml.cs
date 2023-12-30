using App.Connection.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Connection.Views;

public partial class ConnectionControl : UserControl
{
    public ConnectionControl()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<ConnectionControlViewModel>();
    }
}
