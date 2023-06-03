using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace _86boxManager.Tools
{
    internal static class Lifetimes
    {
        public static (T builder, Func<int> after) SetupWithClassicDesktopLifetime<T>(
            this T builder, string[] args, ShutdownMode shutdownMode = ShutdownMode.OnLastWindowClose)
            where T : AppBuilderBase<T>, new()
        {
            var lifetime = new ClassicDesktopStyleApplicationLifetime
            {
                Args = args,
                ShutdownMode = shutdownMode
            };
            builder.SetupWithLifetime(lifetime);
            int After() => lifetime.Start(args);
            return (builder, After);
        }
    }
}