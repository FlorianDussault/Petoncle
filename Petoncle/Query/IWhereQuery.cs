namespace PetoncleDb;

internal interface IWhereQuery :  IQuery
{
    bool HasValue { get; }
}