using App.Connection.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Connection.Views;

public partial class ConnectionState : UserControl
{
    public ConnectionState()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<ConnectionStateViewModel>();
    }
}
