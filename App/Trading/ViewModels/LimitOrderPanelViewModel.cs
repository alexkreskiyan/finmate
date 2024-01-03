using Annium.Logging;
using App.Lib;

namespace App.Trading.ViewModels;

public class LimitOrderPanelViewModel : ViewModelBase, ISingleton, ILogSubject
{
    public ILogger Logger { get; }

    public LimitOrderPanelViewModel(ILogger logger)
    {
        Logger = logger;
    }
}
