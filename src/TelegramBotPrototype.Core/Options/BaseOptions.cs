using Microsoft.Extensions.Options;
using System;
using System.Reflection;

namespace TelegramBotPrototype.Core.Options;

public abstract class BaseOptions<TOptions> where TOptions : BaseOptions<TOptions>, new()
{
    protected virtual string? SectionName => typeof(TOptions).Name;

    protected virtual TOptions CustomConfiguration(IConfiguration configuration, ILogger<TOptions>? logger, params object[] additionalDIObjects)
    {
        return (TOptions)this;
    }

    public static void RegisterOptions(IServiceCollection services, IConfiguration? configuration = null)
    {
        var optionsBuilder = services.AddOptions<TOptions>();

        var wasOverridden = false;

        var customConfigurationDeclaringType = typeof(TOptions).GetMethod(
            nameof(CustomConfiguration), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!
            .DeclaringType;

        if (customConfigurationDeclaringType != typeof(BaseOptions<TOptions>))
        {
            wasOverridden = true;
        }

        if (wasOverridden)
        {
            if (configuration != null)
            {
                optionsBuilder.ConfigureOptDep<TOptions, ILogger<TOptions>>((o, logger) =>
                {
                    ConfigurationBind(configuration, o);
                    o.CustomConfiguration(configuration, logger);
                });
            }
            else
            {
                optionsBuilder.ConfigureDepOptDep<TOptions, IConfiguration, ILogger<TOptions>>((o, config, logger) =>
                {
                    ConfigurationBind(config, o);
                    o.CustomConfiguration(config, logger);
                });
            }
        }
        else
        {
            if (configuration != null)
            {
                optionsBuilder.Configure(o =>
                {
                    ConfigurationBind(configuration, o);
                });
            }
            else
            {
                optionsBuilder.Configure<IConfiguration>((o, config) =>
                {
                    ConfigurationBind(config, o);
                });
            }
        }
    }

    public static void RegisterOptions<TDep>(IServiceCollection services, IConfiguration? configuration = null)
        where TDep : class
    {
        RegisterOptionsFlow(services, configuration,
            optionsBuilder => WithCustomConfigBindFlow<TDep>(configuration, optionsBuilder));
    }

    private static void WithCustomConfigBindFlow<TDep>(IConfiguration? configuration, OptionsBuilder<TOptions> optionsBuilder)
        where TDep : class
    {
        if (configuration != null)
        {
            optionsBuilder.ConfigureOptDep<TOptions, ILogger<TOptions>, TDep>((o, logger, dep) =>
            {
                ConfigurationBind(configuration, o);
                o.CustomConfiguration(GetOptionsConfiguration(configuration, o), logger, dep);
            });
        }
        else
        {
            optionsBuilder.ConfigureDepOptDep<TOptions, IConfiguration, ILogger<TOptions>, TDep>((o, config, logger, dep) =>
            {
                ConfigurationBind(config, o);
                o.CustomConfiguration(GetOptionsConfiguration(config, o), logger, dep);
            });
        }
    }

    private static void RegisterOptionsFlow(IServiceCollection services, IConfiguration? configuration, Action<OptionsBuilder<TOptions>> withCustomBindAction)
    {
        var optionsBuilder = services.AddOptions<TOptions>();

        if (CheckCustomConfigurationNeedToRun())
        {
            withCustomBindAction(optionsBuilder);
        }
        else
        {
            NonCustomConfigBindFlow(configuration, optionsBuilder);
        }

        optionsBuilder.ValidateDataAnnotations();
    }

    private static bool CheckCustomConfigurationNeedToRun()
    {
        var customConfigurationDeclaringType = typeof(TOptions).GetMethod(
            nameof(CustomConfiguration), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)!
            .DeclaringType;

        if (customConfigurationDeclaringType != typeof(BaseOptions<TOptions>))
        {
            return true;
        }

        return false;
    }

    private static void NonCustomConfigBindFlow(IConfiguration? configuration, OptionsBuilder<TOptions> optionsBuilder)
    {
        if (configuration != null)
        {
            optionsBuilder.Configure(o =>
            {
                ConfigurationBind(configuration, o);
            });
        }
        else
        {
            optionsBuilder.Configure<IConfiguration>((o, config) =>
            {
                ConfigurationBind(config, o);
            });
        }
    }

    private static IConfiguration GetOptionsConfiguration(IConfiguration configuration, TOptions o)
    {
        if (!string.IsNullOrEmpty(o.SectionName))
        {
            return configuration.GetSection(o.SectionName);
        }
        else
        {
            return configuration;
        }
    }

    private static void ConfigurationBind(IConfiguration configuration, TOptions o)
    {
        if (!string.IsNullOrEmpty(o.SectionName))
        {
            configuration.Bind(o.SectionName, o);
        }
        else
        {
            configuration.Bind(o);
        }
    }
}
