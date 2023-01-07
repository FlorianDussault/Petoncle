using System;
using System.Linq.Expressions;

namespace PetoncleDb;

internal sealed class DbEntryPoint : EntryPoint
{
    public DbEntryPoint(Petoncle petoncle, Connection connection) : base(petoncle, connection)
    {
    }
}   