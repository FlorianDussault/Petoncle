using System;

namespace PetoncleDb.SqlServer;

public static class Pf
{
    #region Global
    public static bool Like(object column, string value) => throw new PetoncleException($"Use {nameof(Like)} function in expression");
    
    public static bool NotLike(object column, string value) => throw new PetoncleException($"Use {nameof(NotLike)} function in expression");
    
    #endregion
    
    #region Date
    public static string GetDate() => throw new PetoncleException($"Use {nameof(GetDate)} function in expression");
    
    public static int DateDiff(DatePart datePart, DateTime start, DateTime end) => throw new PetoncleException($"Use {nameof(DateDiff)} function in expression");
    
    public static bool IsDate(object column) => throw new PetoncleException($"Use {nameof(IsDate)} function in expression");
    
    public static int Day(object column) => throw new PetoncleException($"Use {nameof(Day)} function in expression");
    
    public static int Month(object column) => throw new PetoncleException($"Use {nameof(Month)} function in expression");
    
    public static int Year(object column) => throw new PetoncleException($"Use {nameof(Year)} function in expression");
    
    public static DateTime DateAdd(object column, DatePart datePart, int increment) => throw new PetoncleException($"Use {nameof(DateAdd)} function in expression");
    
    #endregion
    
    #region String
    public static int? Ascii(object column) => throw new PetoncleException($"Use {nameof(Ascii)} function in expression");
    
    public static string Char(object column) => throw new PetoncleException($"Use {nameof(Char)} function in expression");
    
    public static int? CharIndex(object expression, string search, int? startLocation = null)=> throw new PetoncleException($"Use {nameof(CharIndex)} function in expression");
    
    public static string Concat(params string[] values)=> throw new PetoncleException($"Use {nameof(Concat)} function in expression");
    
    public static string ConcatWs(string separator, params string[] values) => throw new PetoncleException($"Use {nameof(ConcatWs)} function in expression");
    
    public static int? DataLength(object column) => throw new PetoncleException($"Use {nameof(DataLength)} function in expression");
    
    public static string Lower(object column) => throw new PetoncleException($"Use {nameof(Lower)} function in expression");
    
    public static string Upper(object column) => throw new PetoncleException($"Use {nameof(Upper)} function in expression");
    #endregion
}