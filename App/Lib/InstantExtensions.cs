using Annium.NodaTime.Extensions;
using NodaTime;

namespace App.Lib;

public static class InstantExtensions
{
    public static string LocalDateTime(this Instant instant) => instant.InLocal().ToString("dd.MM.yy HH:mm:ss", null);
    public static string LocalTime(this Instant instant) => instant.InLocal().ToString("HH:mm:ss", null);
}
