using System;
using System.Data;

namespace PetoncleDb;

internal sealed class Connection
{
    public DatabaseType DatabaseType { get; }
    private readonly Func<IDbConnection> _connectionProvider;

    internal Connection(DatabaseType databaseType, Func<IDbConnection> connectionProvider)
    {
        DatabaseType = databaseType;
        _connectionProvider = connectionProvider;
    }

    internal IDbConnection GetDbConnection() => _connectionProvider();
}