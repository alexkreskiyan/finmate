using System;
using Annium;
using Annium.Core.DependencyInjection;

namespace App.Lib;

internal static class Provider
{
    private static IServiceProvider? _instance;

    public static void Init(IServiceProvider sp)
    {
        if (_instance is not null)
            throw new InvalidOperationException("Already initialized");

        _instance = sp;
    }

    public static T Resolve<T>()
        where T : notnull
    {
        return _instance.NotNull().Resolve<T>();
    }

    public static T ResolveKeyed<T>(object key)
        where T : notnull
    {
        return _instance.NotNull().ResolveKeyed<T>(key);
    }
}
