using App.Instruments.ViewModels;
using App.Lib;
using Avalonia.Controls;

namespace App.Instruments.Views;

public partial class InstrumentsList : UserControl
{
    public InstrumentsList()
    {
        InitializeComponent();
        DataContext = Provider.Resolve<InstrumentsListViewModel>();
    }
}
