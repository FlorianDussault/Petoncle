using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace PetoncleDb;

internal class ColumnDefinition
{
    internal PropertyInfo Property { get; }
    internal DbColumn DbColumn { get; }
    internal bool IsPrimary { get; }
    internal bool IsAutoIncrement { get; }
    internal bool IsReadOnly { get; }
    
    internal string SqlColumnName { get; }

    private object _setValueAction;
    private object _getValueAction;

    public ColumnDefinition(PropertyInfo property, DbColumn dbColumn, bool isPrimary, bool isAutoIncrement, bool isReadOnly)
    {
        Property = property;
        DbColumn = dbColumn;
        IsPrimary = isPrimary;
        IsAutoIncrement = isAutoIncrement;
        IsReadOnly = isReadOnly;
        SqlColumnName = "[" + (DbColumn.ColumnName ?? property.Name) + "]";
    }

    internal ColumnDefinition SetActions<T>()
    {
        {

            #region Create Setter

            {
                ParameterExpression exInstance = Expression.Parameter(Property.DeclaringType!, "t");
                MemberExpression exMemberAccess = Expression.MakeMemberAccess(exInstance, Property); // t.PropertyName

                ParameterExpression exValue = Expression.Parameter(typeof(object), "p");
                UnaryExpression exConvertedValue = Expression.Convert(exValue, Property.PropertyType);
                BinaryExpression exBody = Expression.Assign(exMemberAccess, exConvertedValue);
                Expression<Action<T, object>>
                    lambda = Expression.Lambda<Action<T, object>>(exBody, exInstance, exValue);
                _setValueAction = lambda.Compile();
            }
        }

        #endregion
        
        #region Create Getter
        {
            DynamicMethod method = new DynamicMethod("cheat", typeof(object),
                new[] { typeof(object) }, typeof(T), true);
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, typeof(T));
            il.Emit(OpCodes.Callvirt, Property.GetGetMethod(true));
            il.Emit(OpCodes.Ret);
            _getValueAction  = (Func<object, object>)method.CreateDelegate(
                typeof(Func<object, object>));
        }

        #endregion

        return this;
    }

    public void SetValue<T>(T obj, object value) => ((Action<T, object>)_setValueAction).Invoke(obj, value);

    // public object GetValue<T>(T obj) => ((Func<object, object>) _getValueAction).Invoke(obj);
    public object GetValue<T>(T obj) => Property.GetGetMethod(false).Invoke(obj, null);
}