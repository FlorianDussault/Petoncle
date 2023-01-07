namespace PetoncleDb;

public interface IDbTable
{
    string TableName { get; }
    string SchemaName { get; }
}