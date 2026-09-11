using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Data;

using System.Linq;
using System.Linq.Expressions;

namespace DbExtensions.Tvp.Parameters
{
    internal static class ArrayBinder<TRow> where TRow : ITableValued
    {
        private static readonly Func<TRow, object[], object[]> _binder = Factory();

        /// <remarks>
        /// <code>
        /// $array[0] = (System.Object).Call $row.get_Property0();
        /// $array[1] = (System.Object).Call $row.get_Property1();
        /// $array[2] = (System.Object).Call $row.get_Property2();
        /// $array
        /// </code>
        /// </remarks>
        private static BlockExpression CreateBodyExpression(ParameterExpression[] args)
        {
            return Expression.Block(TRow.Metadata.Columns.Select(c => new { Ordinal = Expression.Constant(c.Ordinal), Getter = TRow.Type.GetProperty(c.Name).GetMethod }).Select(c => Expression.Assign(Expression.ArrayAccess(args[1], c.Ordinal), Expression.Convert(Expression.Call(args[0], c.Getter), typeof(object)))).Append<Expression>(args[1]));
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

        /// <summary>
        /// Returns a lambda that populates the
        /// provided array with values from the
        /// user object.
        /// </summary>
        public static Func<TRow, object[], object[]> Get()
        {
            return _binder;
        }
    }
}