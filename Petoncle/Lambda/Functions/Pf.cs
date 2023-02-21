﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace PetoncleDb;

public partial class Pf : LambdaFunctionParser
{
    internal override void Parse(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        MethodInfo method =  GetType().GetTypeInfo().GetDeclaredMethod($"Parse{expression.Method.Name}");
        if (method == null)
            throw new NotImplementedException($"Function 'Parse{expression.Method.Name}' is missing");
        method.Invoke(this, new object[] {databaseType, expression, lambdaParser, queryBuilder});
    }
    
    private void ParseLike(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(" LIKE ");
        lambdaParser.ParseExpression(expression.Arguments[1]);
    }

    private void ParseNotLike(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(" NOT LIKE ");
        lambdaParser.ParseExpression(expression.Arguments[1]);
    }
    
    #region Date

    private void ParseIsDate(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" ISDATE(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseDay(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" DAY(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseMonth(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" MONTH(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseYear(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" YEAR(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseDateAdd(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" DATEADD(");
        if (!lambdaParser.GetValueFromExpression(expression.Arguments[1], out object arg1) ||
            arg1 is not DatePart datePart)
            throw new NotImplementedException();
        queryBuilder.Append($"{Enum.GetName(typeof(DatePart), datePart)}, ");
        queryBuilder.Append(expression.Arguments[2]);
        queryBuilder.Append(", ");
        queryBuilder.Append(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    public static string GetDate() => throw new PetoncleException($"Use {nameof(GetDate)} function in expression");
    
    private void ParseGetDate(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder) => queryBuilder.Append(" GETDATE() ");
    
    public static int DateDiff(DatePart datePart, DateTime start, DateTime end) => throw new PetoncleException($"Use {nameof(DateDiff)} function in expression");
    
    private void ParseDateDiff(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" DATEDIFF(");
        if (!lambdaParser.GetValueFromExpression(expression.Arguments[0], out object arg1) ||
            arg1 is not DatePart datePart)
            throw new NotImplementedException();
        queryBuilder.Append($"{Enum.GetName(typeof(DatePart), datePart)}, ");
        queryBuilder.Append(expression.Arguments[1]);
        queryBuilder.Append(", ");
        queryBuilder.Append(expression.Arguments[2]);
        queryBuilder.Append(") ");
    }

    #endregion
}

public partial class Pf
{
    private void ParseAscii(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" ASCII(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseChar(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" CHAR(");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseCharIndex(DatabaseType databaseType, MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" CHARINDEX(");
        lambdaParser.ParseExpression(expression.Arguments[1]);
        queryBuilder.Append(", ");
        lambdaParser.ParseExpression(expression.Arguments[0]);
        if (expression.Arguments.Count > 2)
        {
            queryBuilder.Append(", ");
            lambdaParser.ParseExpression(expression.Arguments[2]);
        }

        queryBuilder.Append(")");
    }
    
    private void ParseConcat(MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" CONCAT(");
        for (int i = 0; i < expression.Arguments.Count; i++)
        {
            lambdaParser.ParseExpression(expression.Arguments[i]);
            if (i + 1 < expression.Arguments.Count)
                queryBuilder.Append(", ");
        }

        queryBuilder.Append(") ");
    }
    
    private void ParseConcatWs(MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" CONCAT_WS(");
        for (int i = 0; i < expression.Arguments.Count; i++)
        {
            lambdaParser.ParseExpression(expression.Arguments[i]);
            if (i + 1 < expression.Arguments.Count)
                queryBuilder.Append(", ");
        }

        queryBuilder.Append(") ");
    }
    
    private void ParseDataLength(MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" DATALENGTH(");
        queryBuilder.Append(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseLower(MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" LOWER(");
        queryBuilder.Append(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
    
    private void ParseUpper(MethodCallExpression expression, LambdaParser lambdaParser, QueryBuilder queryBuilder)
    {
        queryBuilder.Append(" UPPER(");
        queryBuilder.Append(expression.Arguments[0]);
        queryBuilder.Append(") ");
    }
}