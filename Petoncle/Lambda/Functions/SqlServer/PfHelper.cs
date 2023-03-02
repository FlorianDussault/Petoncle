﻿using System;

namespace PetoncleDb.SqlServer;

public static class PfHelper
{
    #region Global
    public static bool Like(this object column, string value) => throw new PetoncleException($"Use {nameof(Like)} function in expression");
    public static bool NotLike(this object column, string value) => throw new PetoncleException($"Use {nameof(NotLike)} function in expression");
    public static object As(this object column, string columnName) => throw new PetoncleException($"Use {nameof(As)} function in expression");
    public static object Count(this object column) => throw new PetoncleException($"Use {nameof(Count)} function in expression");
    public static object Distinct(this object column) => throw new PetoncleException($"Use {nameof(Distinct)} function in expression");
    public static object Max(this object column) => throw new PetoncleException($"Use {nameof(Max)} function in expression");
    public static object Min(this object column) => throw new PetoncleException($"Use {nameof(Min)} function in expression");
    public static object Sum(this object column) => throw new PetoncleException($"Use {nameof(Sum)} function in expression");
    public static object Avg(this object column) => throw new PetoncleException($"Use {nameof(Avg)} function in expression");
    
    #endregion
    
    #region Date
    
    public static bool IsDate(this object column) => throw new PetoncleException($"Use {nameof(IsDate)} function in expression");
    
    public static int Day(this object column) => throw new PetoncleException($"Use {nameof(Day)} function in expression");
    
    public static int Month(this object column) => throw new PetoncleException($"Use {nameof(Month)} function in expression");
    
    public static int Year(this object column) => throw new PetoncleException($"Use {nameof(Year)} function in expression");
    
    public static DateTime DateAdd(this object column, DatePart datePart, int increment) => throw new PetoncleException($"Use {nameof(DateAdd)} function in expression");
    
    #endregion
    
    #region String
    public static int? Ascii(this object column) => throw new PetoncleException($"Use {nameof(Ascii)} function in expression");
    
    public static string Char(this object column) => throw new PetoncleException($"Use {nameof(Char)} function in expression");
    
    public static int? CharIndex(this object expression, string search, int? startLocation = null)=> throw new PetoncleException($"Use {nameof(CharIndex)} function in expression");
    
    public static int? DataLength(this object column) => throw new PetoncleException($"Use {nameof(DataLength)} function in expression");
    
    public static string Lower(this object column) => throw new PetoncleException($"Use {nameof(Lower)} function in expression");
    
    public static string Upper(this object column) => throw new PetoncleException($"Use {nameof(Upper)} function in expression");
    
    #endregion
}