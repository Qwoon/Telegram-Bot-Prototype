using System.Linq;
using System.Threading;
using TelegramBotPrototype.Data.Configuration;
using TelegramBotPrototype.Data.Entities;

namespace TelegramBotPrototype.Data;

public class TelegramBotPrototypeContext : DbContext
{
    public TelegramBotPrototypeContext(DbContextOptions<TelegramBotPrototypeContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureDatabase();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();
        var modifiedEntities = ChangeTracker.Entries().Select(x => x.State == EntityState.Modified);
        return base.SaveChangesAsync(cancellationToken);
    }

    public virtual DbSet<DailyNote> DailyNotes { get; set; }
    public virtual DbSet<BotChat> BotChats { get; set; }
}
