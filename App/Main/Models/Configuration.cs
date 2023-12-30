using System;
using Annium.Finance.Providers.Abstractions.Domain.Models;

namespace App.Main.Models;

public sealed record Configuration
{
    public UserSettings Settings { get; init; } = default!;
    public string[] Symbols { get; init; } = Array.Empty<string>();
}
