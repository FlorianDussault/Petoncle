using System;
using System.Data;
using System.Linq.Expressions;

namespace PetoncleDb;
//
// public class Transaction : EntryPoint, IDisposable
// {
//     public Transaction(DatabaseType databaseType, IDbConnection dbConnection) : base()
//     {
//         
//     }
//
//     public void Dispose()
//     {
//     }
//
//     public override IPetoncleEnumerable<T> Select<T>()
//     {
//         throw new NotImplementedException();
//     }
//
//     public override IPetoncleEnumerable<T> Select<T>(string tableName)
//     {
//         throw new NotImplementedException();
//     }
//
//     public override IPetoncleEnumerable<T> Select<T>(Expression<Func<T, bool>> where)
//     {
//         throw new NotImplementedException();
//     }
//
//     public override IPetoncleEnumerable<T> Select<T>(string tableName, Expression<Func<T, bool>> where)
//     {
//         throw new NotImplementedException();
//     }
//
//     public override int Insert<T>(T obj)
//     {
//         throw new NotImplementedException();
//     }
//
//     public override int Delete<T>()
//     {
//         throw new NotImplementedException();
//     }
// }