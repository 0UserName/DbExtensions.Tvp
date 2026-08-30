using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Data;

using System.Linq;
using System.Linq.Expressions;

using System.Reflection;

namespace DbExtensions.Tvp.Parameters
{
    internal static class DataRowBinder<TRow> where TRow : ITableValued
    {
        private static readonly MethodInfo _setField = typeof(DataRowExtensions).GetMethods().First(m => m.Name == nameof(DataRowExtensions.SetField));

        private static readonly Func<TRow, DataRow, DataRow> _binder = Create();

        /// <summary>
        /// 
        /// </summary>
        private static MethodInfo GetMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }

        /// <summary>
        /// 
        /// </summary>
        private static MethodInfo GetIsDBNull()
        {
            return GetMethod(TRow.Type, nameof(ITableValued.IsDBNull));
        }

        /// <summary>
        /// 
        /// </summary>
        private static MethodInfo GetGetValue(Type typeArgument)
        {
            return GetMethod(TRow.Type, nameof(ITableValued.GetValue)).MakeGenericMethod(typeArgument);
        }

        private static Func<TRow, DataRow, DataRow> Create()
        {
            Expression[] expressions = new
            Expression
            [TRow.Metadata.Columns.Length + 1];

            ParameterExpression[] _args = new
            ParameterExpression[]
            {
                Expression.Parameter(TRow.Type), Expression.Parameter(typeof(DataRow))
            };

            expressions[TRow.Metadata.Columns.Length] = _args[1]; // Returns the DataRow argument.

            foreach (IColumnInternalMetadata metadata in TRow.Metadata.Columns)
            {
                ConstantExpression ordinal = Expression.Constant(metadata.Ordinal);

                MethodCallExpression isDBNull = Expression.Call(instance: _args[0], arguments: ordinal, method: GetIsDBNull());
                MethodCallExpression getValue = Expression.Call(instance: _args[0], arguments: ordinal, method: GetGetValue(metadata.Type));
                MethodCallExpression setField = Expression.Call(
                    _setField.MakeGenericMethod(metadata.Type), _args[1], ordinal, getValue);

                expressions[metadata.Ordinal] = Expression.IfThen(Expression.IsFalse(isDBNull), setField);
            }

            Expression<Func<TRow, DataRow, DataRow>> lambda = Expression.Lambda
                      <Func<TRow, DataRow, DataRow>>
                      (Expression.Block(expressions), _args);

            return lambda.Compile();
        }

        /// <summary>
        /// Returns a lambda that populates the data row from the class properties.
        /// </summary>
        public static Func<TRow, DataRow, DataRow> Get()
        {
            return _binder;
        }
    }
}