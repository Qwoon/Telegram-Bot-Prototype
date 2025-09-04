namespace TelegramBotPrototype.EFCore.Design;

public abstract class SqlContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    /// <inheritdoc/>
    public virtual TContext CreateDbContext(string[] args)
    {
        // Debug();
        var host = CreateHostBuilder(args).Build();
        return (TContext)host.Services.GetRequiredService<DbContext>();
    }

    protected virtual IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices(ConfigureServices);
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DbContext, TContext>(ConfigureDbContext);
    }

    protected virtual void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.ReplaceService<IMigrationCommandExecutor, SqlMigrationCommandExecutor>();
    }

    /// <summary>
    ///     Call to wait for debugger to be attached before proceeding any further.
    /// </summary>
    protected static void Debug()
    {
        Debugger.Launch();
        if (Debugger.IsAttached)
            return;
        Console.WriteLine("Current ProcessID: {0}", Environment.ProcessId);
        Console.WriteLine("Waiting for debugger to attach...");
        while (!Debugger.IsAttached)
            Thread.Sleep(100);
        Console.WriteLine("Debugger attached!");
    }
}
