using DbExtensions.Tvp.Metadata;
using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;

namespace DbExtensions.Tvp.Binders
{
    internal static class PropertyBinder<TRow> where TRow : ITableValued
    {
        private static readonly Delegate[] _cache = CompileGetFieldValueBinders();

        /// <summary>
        /// Gets a value that
        /// indicates whether
        /// the column is set
        /// to null.
        /// </summary>
        /// 
        /// <remarks>
        /// <code>
        /// bool IsDBNull(TRow row, int ordinal)
        /// </code>
        /// </remarks>
        public static readonly Func<TRow, int, bool> IsDBNull = CompileIsDBNullBinder();

        /// <summary>
        /// Gets the value of the specified
        /// column as an instance of Object.
        /// </summary>
        /// 
        /// <remarks>
        /// <code>
        /// object GetValue(TRow row, int ordinal)
        /// </code>
        /// </remarks>
        public static readonly Func<TRow, int, object> GetValue = CompileGetValueBinder();

        /// <summary>
        /// Creates a SwitchExpression that accesses row members by their ordinal values.
        /// </summary>
        private static SwitchExpression CreatePropertySwitchExpression(Type expressionType, ParameterExpression ordinal, IEnumerable<SwitchCase> cases)
        {
            return Expression.Switch(ordinal, Expression.Throw(Expression.New(typeof(IndexOutOfRangeException).GetConstructor(new[] { typeof(string) }), Expression.Call(typeof(string).GetMethod(nameof(string.Format), new Type[] { typeof(string), typeof(object) }), Expression.Constant("Column with ordinal {0} does not exist"), Expression.Convert(ordinal, typeof(object)))), expressionType), cases.ToArray());
        }

        /// <remarks>
        /// <code>
        /// .Switch($ordinal) {
        /// .Case(0):
        ///         $row.Property0
        /// .Case(1):
        ///         $row.Property1
        /// .Case(2):
        ///         False
        /// .Default:
        ///         .Throw.New System.IndexOutOfRangeException(.Call System.String.Format("Column with ordinal {0} does not exist", (System.Object)$ordinal))
        /// }
        /// </code>
        /// </remarks>
        private static Func<TRow, int, bool> CompileIsDBNullBinder()
        {
            ParameterExpression[] args = new
            ParameterExpression[]
            {
                Expression.Parameter(TRow.Type, "row"), Expression.Parameter(typeof(int), "ordinal")
            };

            Expression<Func<TRow, int, bool>> lambda = Expression.Lambda
                      <Func<TRow, int, bool>>
                      (CreatePropertySwitchExpression(typeof(bool), args[1], TRow.Metadata.Columns.Select(c => new { Ordinal = Expression.Constant(c.Ordinal), Property = TRow.Type.GetProperty(c.Name) }).Select(c => Expression.SwitchCase(MetadataProvider<TRow>.GetUnderlyingType(c.Property, out _) ? Expression.Equal(Expression.Property(args[0], c.Property), Expression.Constant(default)) : Expression.Constant(false), c.Ordinal))), args);

            return lambda.Compile();
        }

        /// <remarks>
        /// <code>
        /// .Switch($ordinal) {
        /// .Case(0):
        ///         (System.Object)$row.Property0
        /// .Case(1):
        ///         (System.Object)$row.Property1
        /// .Case(2):
        ///         (System.Object)$row.Property2
        /// .Default:
        ///         .Throw.New System.IndexOutOfRangeException(.Call System.String.Format("Column with ordinal {0} does not exist", (System.Object)$ordinal))
        /// }
        /// </code>
        /// </remarks>
        private static Func<TRow, int, object> CompileGetValueBinder()
        {
            ParameterExpression[] args = new
            ParameterExpression[]
            {
                Expression.Parameter(TRow.Type, "row"), Expression.Parameter(typeof(int), "ordinal")
            };

            Expression<Func<TRow, int, object>> lambda = Expression.Lambda
                      <Func<TRow, int, object>>
                      (CreatePropertySwitchExpression(typeof(object), args[1], TRow.Metadata.Columns.Select(c => Expression.SwitchCase(Expression.Convert(Expression.Property(args[0], TRow.Type.GetProperty(c.Name)), typeof(object)), Expression.Constant(c.Ordinal)))), args);

            return lambda.Compile();
        }

        /// <remarks>
        /// <code>
        /// [0] = () => $row.Property0
        /// [1] = () => $row.Property1
        /// [2] = () => $row.Property2
        /// </code>
        /// </remarks>
        private static Delegate[] CompileGetFieldValueBinders()
        {
            ParameterExpression row = Expression.Parameter(TRow.Type, "row");

            return TRow.Metadata.Columns.Select(c => Expression.Lambda(Expression.Property(row, TRow.Type.GetProperty(c.Name)), row).Compile()).ToArray();
        }

        /// <summary>
        /// Gets the value of the specified column as the requested type.
        /// </summary>
        public static T GetFieldValue<T>(TRow row, int ordinal)
        {
            return _cache[ordinal] is Func<TRow, T> l ? l(row) : (T)GetValue(row, ordinal);
        }
    }
}