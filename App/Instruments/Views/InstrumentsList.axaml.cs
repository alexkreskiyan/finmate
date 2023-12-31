using App.Instruments.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Instruments.Views;

public partial class InstrumentsList : UserControl
{
    private readonly InstrumentsListViewModel _vm;

    public InstrumentsList()
    {
        InitializeComponent();
        DataContext = _vm = Provider.Resolve<InstrumentsListViewModel>();
    }

    private void HandleSelectionChanged(object? sender, SelectionChangedEventArgs e) =>
        _vm.HandleSelectionChanged(sender, e);
}
