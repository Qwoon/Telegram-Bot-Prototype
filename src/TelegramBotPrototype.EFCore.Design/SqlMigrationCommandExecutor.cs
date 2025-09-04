namespace TelegramBotPrototype.EFCore.Design;

/// <summary>
/// Filters out commands migrations history altering as they already part of the SQL script.
/// </summary>
[SuppressMessage("Usage", "EF1001", Justification = "Internal EF Core API usage.")]
public class SqlMigrationCommandExecutor : MigrationCommandExecutor
{
    private readonly string insertHistory;
    private readonly string deleteHistory;

    public SqlMigrationCommandExecutor(ISqlGenerationHelper sqlGenerationHelper, IExecutionStrategy executionStrategy) : base(executionStrategy)
    {
        ArgumentNullException.ThrowIfNull(sqlGenerationHelper);
        var table = sqlGenerationHelper.DelimitIdentifier("__EFMigrationsHistory", schema: null);
        insertHistory = $"INSERT INTO {table} (";
        deleteHistory = $"DELETE FROM {table} WHERE";
    }

    public override void ExecuteNonQuery(IEnumerable<MigrationCommand> migrationCommands, IRelationalConnection connection)
    {
        base.ExecuteNonQuery(migrationCommands.Where(Match), connection);
    }

    public override Task ExecuteNonQueryAsync(IEnumerable<MigrationCommand> migrationCommands, IRelationalConnection connection, CancellationToken cancellationToken = default)
    {
        return base.ExecuteNonQueryAsync(migrationCommands.Where(Match), connection, cancellationToken);
    }

    private bool Match(MigrationCommand command)
    {
        return !IsHistory(command.CommandText);
    }

    private bool IsHistory(string sql)
    {
        return sql.StartsWith(insertHistory, StringComparison.Ordinal)
               || sql.StartsWith(deleteHistory, StringComparison.Ordinal);
    }
}
