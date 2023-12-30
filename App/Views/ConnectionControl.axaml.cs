using App.ViewModels;
using Avalonia.Controls;

namespace App.Views;

public partial class ConnectionControl : UserControl
{
    public ConnectionControl()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<ConnectionControlViewModel>();
    }
}
