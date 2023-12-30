using Annium.Logging;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase, ILogSubject
{
    public ILogger Logger { get; }

    public MainWindowViewModel(ILogger logger)
    {
        Logger = logger;
        this.Debug("create");
        /*
         * Config:
         * 1. Credentials
         * 2. Working instruments
         * UI
         * 1. Connector state indicator, connector switch (top right)
         * 2. Controls (order type 6 btns, qty, price, level price, reduce only checkbox, buy/sell) (below state)
         * 2. Instruments with current tickers (bid/ask prices) (second column from left)
         * 3. Last 10 candles of selected instrument (first one is selected by default) (below instruments)
         * 4. Assets (left top)
         * 5. Active positions (with orientation range) (right to assets)
         * 6. Active orders (right to positions)
         * 7. Last 20 orders (below active orders)
         * 8. Last 50 trades (below assets/positions)
         */
    }
}
