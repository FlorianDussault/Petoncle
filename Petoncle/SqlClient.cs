using System;
using System.Data;

namespace PetoncleDb;

internal class SqlClient : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private readonly IDbCommand _dbCommand;
    
    public SqlClient(Connection connection)
    {
        _dbConnection = connection.GetDbConnection();
        _dbCommand = _dbConnection.CreateCommand();
        _dbConnection.Open();
    }
    
    public IDataReader ExecuteQuery(QueryBuilder queryBuilder)
    {
        _dbCommand.CommandText = queryBuilder.GetQuery();

        foreach (SqlArgument argument in queryBuilder.Arguments)
        {
            IDbDataParameter parameter = _dbCommand.CreateParameter();
            parameter.ParameterName = argument.Name;
            parameter.Value = argument.Value;
            _dbCommand.Parameters.Add(parameter);
        }
        
        try
        {
            return _dbCommand.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.SingleResult);
        }
        catch (Exception ex)
        {
            throw new PetoncleException($"Error with the following query '{_dbCommand.CommandText}'", ex);
        }
    }
    
    public int ExecuteNonQuery(QueryBuilder queryBuilder)
    {
        _dbCommand.CommandText = queryBuilder.GetQuery();

        foreach (SqlArgument argument in queryBuilder.Arguments)
        {
            IDbDataParameter parameter = _dbCommand.CreateParameter();
            parameter.ParameterName = argument.Name;
            parameter.Value = argument.Value ?? DBNull.Value;
            _dbCommand.Parameters.Add(parameter);
        }
        
        try
        {
            return _dbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new PetoncleException($"Error with the following query '{_dbCommand.CommandText}'", ex);
        }
    }

    public void Dispose()
    {
        _dbCommand?.Dispose();
        _dbConnection?.Dispose();
    }

    public object ExecuteScalar(QueryBuilder queryBuilder)
    {
        _dbCommand.CommandText = queryBuilder.GetQuery();

        foreach (SqlArgument argument in queryBuilder.Arguments)
        {
            IDbDataParameter parameter = _dbCommand.CreateParameter();
            parameter.ParameterName = argument.Name;
            parameter.Value = argument.Value ?? DBNull.Value;
            _dbCommand.Parameters.Add(parameter);
        }
        
        try
        {
            return _dbCommand.ExecuteScalar();
        }
        catch (Exception ex)
        {
            throw new PetoncleException($"Error with the following query '{_dbCommand.CommandText}'", ex);
        }
    }
}