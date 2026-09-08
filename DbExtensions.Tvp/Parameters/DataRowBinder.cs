using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Data;

using System.Linq;
using System.Linq.Expressions;

namespace DbExtensions.Tvp.Parameters
{
    internal static class DataRowBinder<TRow> where TRow : ITableValued
    {
        private static readonly Action<DataTable, TRow, object[]> _binder = Factory();

        /// <remarks>
        /// <code>
        /// $buffer[0] = .Call $row.GetValue(0);
        /// $buffer[1] = .Call $row.GetValue(1);
        /// $buffer[2] = .Call $row.GetValue(2);
        /// $buffer[3] = .Call $row.GetValue(3);
        /// $buffer[4] = .Call $row.GetValue(4);
        /// $buffer[5] = .Call $row.GetValue(5);
        /// .Call($table.Rows).Add($buffer)
        /// </code>
        /// 
        /// Has better performance than populating the buffer in a loop.
        /// </remarks>
        private static BlockExpression CreateBodyExpression(ParameterExpression[] args)
        {
            return Expression.Block(TRow.Metadata.Columns.Select(c => Expression.Constant(c.Ordinal)).Select(o => Expression.Assign(Expression.ArrayAccess(args[2], o), Expression.Call(args[1], TRow.Type.GetMethod(nameof(ITableValued.GetValue)).MakeGenericMethod(typeof(object)), o))).Append<Expression>(Expression.Call(Expression.Property(args[0], nameof(DataTable.Rows)), typeof(DataRowCollection).GetMethod(nameof(DataRowCollection.Add), new[] { typeof(object[]) }), args[2])));
        }

        private static Action<DataTable, TRow, object[]> Factory()
        {
            ParameterExpression[] args = new
            ParameterExpression[]
            {
                Expression.Parameter(typeof(DataTable), "table"), Expression.Parameter(TRow.Type, "row"), Expression.Parameter(typeof(object[]), "buffer")
            };

            Expression<Action<DataTable, TRow, object[]>> lambda = Expression.Lambda
                      <Action<DataTable, TRow, object[]>>
                      (CreateBodyExpression(args), args);

            return lambda.Compile();
        }

        /// <summary>
        /// Returns a lambda that adds a new row to the table, populating
        /// it with values from the user object using the provided buffer.
        /// </summary>
        public static Action<DataTable, TRow, object[]> Get()
        {
            return _binder;
        }
    }
}