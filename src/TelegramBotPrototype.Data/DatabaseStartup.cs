using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TelegramBotPrototype.EFCore.Design;

namespace TelegramBotPrototype.Data;

public static class DatabaseStartup
{
    private const string DefaultConnection = "name=MySql";

    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextPool<DbContext, TelegramBotPrototypeContext>(SetupMySql);

        UpdateDatabase(services);
    }

    internal static void SetupMySql(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
        SetupMySql(provider, builder, DefaultConnection);
    }

    internal static void SetupMySql(IServiceProvider provider, DbContextOptionsBuilder builder, string connection, ServerVersion version = default)
    {
        if (TryGetConnectionName(connection, out var name))
            connection = provider.GetRequiredService<IConfiguration>().GetConnectionString(name)
                         ?? throw new("Unable to resolve connection string: " + name);

        var dataSource = BuildDataSource(connection);
        builder
            .UseMySql(dataSource, version ?? GetServerVersion(dataSource), ConfigureMySql)
            .ReplaceService<IMigrationCommandExecutor, SqlMigrationCommandExecutor>();

        EnableLogs(builder);
    }

    [Conditional("DEBUG")]
    private static void EnableLogs(DbContextOptionsBuilder builder)
    {
        builder
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    private static void UpdateDatabase(IServiceCollection services)
    {
        using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = scope.ServiceProvider.GetService<TelegramBotPrototypeContext>();
        context.Database.Migrate();
    }

    private static ServerVersion GetServerVersion(MySqlDataSource dataSource)
    {
        using var connection = dataSource.OpenConnection();
        return ServerVersion.AutoDetect(connection);
    }

    private static MySqlDataSource BuildDataSource(string connection)
    {
        var builder = new MySqlDataSourceBuilder(connection)
        {
            ConnectionStringBuilder =
            {
                Pooling = false, // use connection pooling from EF Core
                UseAffectedRows = false, // required by Pomelo.EntityFrameworkCore.MySql
                AllowUserVariables = true, // required by Pomelo.EntityFrameworkCore.MySql
                SslMode = MySqlSslMode.Required
            }
        };

        return builder.Build();
    }

    private static void ConfigureMySql(MySqlDbContextOptionsBuilder builder)
    {
        builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    }

    private static bool TryGetConnectionName(ReadOnlySpan<char> connection, [MaybeNullWhen(false)] out string name)
    {
        name = default;
        var index = connection.IndexOf('=');
        if (index <= 0)
            return false;

        var key = connection[..index].Trim();
        var value = connection[(index + 1)..].Trim();
        if (!key.Equals("name", StringComparison.OrdinalIgnoreCase) || value.Contains('='))
            return false;

        name = value.ToString();
        return true;
    }
}
