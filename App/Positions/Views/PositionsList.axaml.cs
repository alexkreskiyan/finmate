using App.Lib;
using App.Positions.ViewModels;
using Avalonia.Controls;

namespace App.Positions.Views;

public partial class PositionsList : UserControl
{
    public PositionsList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<PositionsListViewModel>();
    }
}
