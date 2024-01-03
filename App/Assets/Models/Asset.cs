using ReactiveUI;

namespace App.Assets.Models;

public sealed record Asset : ReactiveRecord
{
    public string Resource { get; }

    public decimal Free
    {
        get => _free;
        set => this.RaiseAndSetIfChanged(ref _free, value);
    }

    public decimal Locked
    {
        get => _locked;
        set => this.RaiseAndSetIfChanged(ref _locked, value);
    }

    private decimal _free;
    private decimal _locked;

    public Asset(string resource, decimal free, decimal locked)
    {
        Resource = resource;
        _free = free;
        _locked = locked;
    }
}
