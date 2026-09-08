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
        /// $buffer[0] = (System.Object).Call $row.get_Property5();
        /// $buffer[1] = (System.Object).Call $row.get_Property4();
        /// $buffer[2] = (System.Object).Call $row.get_Property3();
        /// $buffer[3] = (System.Object).Call $row.get_Property2();
        /// $buffer[4] = (System.Object).Call $row.get_Property1();
        /// $buffer[5] = (System.Object).Call $row.get_Property0();
        /// .Call($table.Rows).Add($buffer)
        /// </code>
        /// </remarks>
        private static BlockExpression CreateBodyExpression(ParameterExpression[] args)
        {
            return Expression.Block(TRow.Metadata.Columns.Select(c => new { Ordinal = Expression.Constant(c.Ordinal), Getter = TRow.Type.GetProperty(c.Name).GetMethod }).Select(c => Expression.Assign(Expression.ArrayAccess(args[2], c.Ordinal), Expression.Convert(Expression.Call(args[1], c.Getter), typeof(object)))).Append<Expression>(Expression.Call(Expression.Property(args[0], nameof(DataTable.Rows)), typeof(DataRowCollection).GetMethod(nameof(DataRowCollection.Add), new[] { typeof(object[]) }), args[2])));
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