using System.Linq.Expressions;

namespace PetoncleDb;

public abstract class LambdaFunctionParser
{
    internal abstract void Parse(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder);
}