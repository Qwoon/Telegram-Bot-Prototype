using Microsoft.Extensions.Options;
using System;

namespace TelegramBotPrototype.Core.Options;

public static class BaseOptionsExtensions
{
    public static IServiceCollection RegisterBaseOptions<TOptions>(this IServiceCollection services, IConfiguration? configuration = null)
        where TOptions : BaseOptions<TOptions>, new()
    {
        BaseOptions<TOptions>.RegisterOptions(services, configuration);
        return services;
    }

    public static IServiceCollection RegisterBaseOptions<TOptions, TDep>(this IServiceCollection services, IConfiguration? configuration = null)
        where TOptions : BaseOptions<TOptions>, new()
        where TDep : class
    {
        BaseOptions<TOptions>.RegisterOptions<TDep>(services, configuration);
        return services;
    }

    internal static OptionsBuilder<TOptions> ConfigureOptDep<TOptions, TOptDep>(
        this OptionsBuilder<TOptions> optionsBuilder,
        Action<TOptions, TOptDep?> configureOptions)
    where TOptions : BaseOptions<TOptions>, new()
    where TOptDep : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TOptDep>(
                    optionsBuilder.Name,
                    sp.GetService<TOptDep>(),
                    configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureOptDep<TOptions, TOptDep, TDep>(
        this OptionsBuilder<TOptions> optionsBuilder,
        Action<TOptions, TOptDep?, TDep> configureOptions)
            where TOptions : BaseOptions<TOptions>, new()
            where TOptDep : class
            where TDep : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TOptDep, TDep>(
                    optionsBuilder.Name, sp.GetService<TOptDep>(),
                    sp.GetRequiredService<TDep>(),
                    configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureOptDep<TOptions, TOptDep, TDep, TDep2>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TOptDep?, TDep, TDep2> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TOptDep : class
        where TDep : class
        where TDep2 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TOptDep, TDep, TDep2>(optionsBuilder.Name, sp.GetService<TOptDep>(), sp.GetRequiredService<TDep>(), sp.GetRequiredService<TDep2>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureOptDep<TOptions, TOptDep, TDep, TDep2, TDep3>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TOptDep?, TDep, TDep2, TDep3> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TOptDep : class
        where TDep : class
        where TDep2 : class
        where TDep3 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TOptDep, TDep, TDep2, TDep3>(optionsBuilder.Name, sp.GetService<TOptDep>(), sp.GetRequiredService<TDep>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureOptDepDepDepDepDep<TOptions, TOptDep, TDep, TDep2, TDep3, TDep4>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TOptDep?, TDep, TDep2, TDep3> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TOptDep : class
        where TDep : class
        where TDep2 : class
        where TDep3 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TOptDep, TDep, TDep2, TDep3>(optionsBuilder.Name, sp.GetService<TOptDep>(), sp.GetRequiredService<TDep>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureDepOptDep<TOptions, TDep, TOptDep>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TDep, TOptDep?> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TDep : class
        where TOptDep : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep, TOptDep>(optionsBuilder.Name, sp.GetRequiredService<TDep>(), sp.GetService<TOptDep>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureDepOptDep<TOptions, TDep, TOptDep, TDep2>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TDep, TOptDep?, TDep2> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TDep : class
        where TOptDep : class
        where TDep2 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep, TOptDep, TDep2>(optionsBuilder.Name, sp.GetRequiredService<TDep>(), sp.GetService<TOptDep>(), sp.GetRequiredService<TDep2>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureDepOptDep<TOptions, TDep, TOptDep, TDep2, TDep3>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TDep, TOptDep?, TDep2, TDep3> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TDep : class
        where TOptDep : class
        where TDep2 : class
        where TDep3 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep, TOptDep, TDep2, TDep3>(optionsBuilder.Name, sp.GetRequiredService<TDep>(), sp.GetService<TOptDep>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions));

        return optionsBuilder;
    }

    internal static OptionsBuilder<TOptions> ConfigureDepOptDep<TOptions, TDep, TOptDep, TDep2, TDep3, TDep4>(this OptionsBuilder<TOptions> optionsBuilder, Action<TOptions, TDep, TOptDep?, TDep2, TDep3, TDep4> configureOptions)
        where TOptions : BaseOptions<TOptions>, new()
        where TDep : class
        where TOptDep : class
        where TDep2 : class
        where TDep3 : class
        where TDep4 : class
    {
        optionsBuilder.Services.AddTransient<IConfigureOptions<TOptions>>(sp =>
                new ConfigureNamedOptions<TOptions, TDep, TOptDep, TDep2, TDep3, TDep4>(optionsBuilder.Name, sp.GetRequiredService<TDep>(), sp.GetService<TOptDep>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), configureOptions));

        return optionsBuilder;
    }

}
