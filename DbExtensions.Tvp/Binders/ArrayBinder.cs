using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Data;

using System.Linq;
using System.Linq.Expressions;

namespace DbExtensions.Tvp.Binders
{
    internal static class ArrayBinder<TRow> where TRow : ITableValued
    {
        /// <summary>
        /// Populates an array of objects
        /// with the column values of the
        /// current row.
        /// </summary>
        /// 
        /// <remarks>
        /// <code>
        /// object[] GetValues(TRow row, object[] values)
        /// </code>
        /// </remarks>
        public readonly static Func<TRow, object[], object[]> GetValues = Factory();

        /// <remarks>
        /// <code>
        /// $array[0] = (System.Object)$row.Property0;
        /// $array[1] = (System.Object)$row.Property1;
        /// $array[2] = (System.Object)$row.Property2;
        /// $array
        /// </code>
        /// </remarks>
        private static BlockExpression CreateBodyExpression(ParameterExpression[] args)
        {
            return Expression.Block(TRow.Metadata.Columns.Select(c => Expression.Assign(Expression.ArrayAccess(args[1], Expression.Constant(c.Ordinal)), Expression.Convert(Expression.Property(args[0], TRow.Type.GetProperty(c.Name)), typeof(object)))).Append<Expression>(args[1]));
        }

        private static Func<TRow, object[], object[]> Factory()
        {
            ParameterExpression[] args = new
            ParameterExpression[]
            {
                Expression.Parameter(TRow.Type, "row"), Expression.Parameter(typeof(object[]), "array")
            };

            Expression<Func<TRow, object[], object[]>> lambda = Expression.Lambda
                      <Func<TRow, object[], object[]>>
                      (CreateBodyExpression(args), args);

            return lambda.Compile();
        }
    }
}