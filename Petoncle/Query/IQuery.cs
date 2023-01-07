namespace PetoncleDb;

internal interface IQuery
{
    void Build(ref QueryBuilder queryBuilder);
}