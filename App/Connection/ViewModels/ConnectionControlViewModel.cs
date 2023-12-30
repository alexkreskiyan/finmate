using System;
using System.Threading.Tasks;
using Annium;
using Annium.Logging;
using Annium.Threading;
using App.Lib;
using NodaTime;
using ReactiveUI;

namespace App.Connection.ViewModels;

public class ConnectionControlViewModel : ViewModelBase, ISingleton, IAsyncDisposable, IDisposable, ILogSubject
{
    private static readonly DateTimeZone UtcTz = DateTimeZone.Utc;
    public ILogger Logger { get; }
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }

    private readonly ITimeProvider _timeProvider;
    private string _text = string.Empty;

    public ConnectionControlViewModel(Main.Services.Connection connection, ITimeProvider timeProvider, ILogger logger)
    {
        Logger = logger;
        _timeProvider = timeProvider;
        Timers.Async(SetTime, 0, 100);
    }

    private ValueTask SetTime()
    {
        Text = _timeProvider.Now.InZone(UtcTz).LocalDateTime.ToString("dd.MM.yy HH:mm:ss.fff", null);

        return ValueTask.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        this.Info("done");

        return ValueTask.CompletedTask;
    }

    public void Dispose()
    {
        this.Info("done");
    }
}
