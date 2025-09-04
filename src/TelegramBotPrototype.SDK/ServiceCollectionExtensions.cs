using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using TelegramBotPrototype.Core.Options;
using TelegramBotPrototype.SDK.Interfaces;
using TelegramBotPrototype.SDK.Options;
using TelegramBotPrototype.SDK.Services.Implementations;
using TelegramBotPrototype.SDK.Services.Interfaces;

namespace TelegramBotPrototype.SDK;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSdkServices(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterAutoMapper()
            .RegisterOptions()
            .AddResourceServices()
            .AddSingleton<UserStateManager>()
            .AddSingleton<MessageCommandRouter>()
            .AddSingleton<IBotUpdateHandler, BotUpdateHandler>();

        services.AddHttpClients(configuration);

        return services;
    }

    private static IServiceCollection AddResourceServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDailyNoteService, DailyNoteService>()
            .AddSingleton<IBotUserService, BotUserService>();
    }

    private static IServiceCollection RegisterOptions(this IServiceCollection services)
    {
        return services
            .RegisterBaseOptions<BotOptions>()
            .RegisterBaseOptions<TelegramApiOptions>();
    }

    private static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("TelegramApiOptions");
        var options = configurationSection.Get<TelegramApiOptions>();

        services.AddHttpClient("telegram-webhook")
            .RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(options.ApiToken, httpClient));
    }

    private static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        var assemblyTypes = new[]
        {
            typeof(ServiceCollectionExtensions)
        };
        services.AddAutoMapper(assemblyTypes);

        return services;
    }
}
