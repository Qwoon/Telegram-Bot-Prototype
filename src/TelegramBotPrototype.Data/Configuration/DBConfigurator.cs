using TelegramBotPrototype.Data.Entities;

namespace TelegramBotPrototype.Data.Configuration;

/// <summary>
/// Represents extension method for configuring the database tables & relations.
/// Please stick to the following template when configuring new resoures:
/// 1. Primary Key
/// 2. Properties
/// 3. Relations:
///     3.1. HasOne > HasMany, so make sure to write relations where the foreign key is located.
/// </summary>
public static class DBConfigurator
{
    public static void ConfigureDatabase(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DailyNote>(entity =>
        {
            entity.HasKey(x => x.Id);
        });

        modelBuilder.Entity<BotChat>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.ChatId).IsUnique();
        });
    }
}
