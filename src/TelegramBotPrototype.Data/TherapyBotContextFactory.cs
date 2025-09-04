using TelegramBotPrototype.Data;
using TelegramBotPrototype.EFCore.Design;

namespace BitBonusRadar.Data;

public class TelegramBotPrototypeContextFactory : SqlContextFactory<TelegramBotPrototypeContext>
{
    protected override void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
        Console.WriteLine("Hello world!");
        base.ConfigureDbContext(provider, builder);

        const string connection = "server=localhost";
        var version = ServerVersion.Parse("8.0.21-mysql");
        DatabaseStartup.SetupMySql(provider, builder, connection, version);
    }
}
