using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Linq.Expressions;

using System.Reflection;

using System.Runtime.CompilerServices;

namespace DbExtensions.Tvp.Metadata
{
    internal static class PropertyBinder<TRow> where TRow : ITableValued
    {
        private static readonly
            Delegate[] _cache = new
            Delegate
            [TRow.Metadata.Columns.Length * 3];

        /// <summary>
        /// Compiles a null-check lambda 
        /// expression for the specified
        /// column.
        /// </summary>
        private static Func<TRow, bool> CompileIsDBNullBinder(ParameterExpression instance, PropertyInfo property)
        {
            Expression<Func<TRow, bool>> lambda = Expression.Lambda
                      <Func<TRow, bool>>
                      (Expression.Equal(Expression.Property(instance, property), Expression.Constant(default)), instance);

            return lambda.Compile();
        }

        /// <summary>
        /// Adds a null-check lambda to the cache: compiles a lambda expression or uses a stub lambda if the column type is not nullable.
        /// </summary>
        private static void AddIsDBNullBinder(ParameterExpression instance, int ordinal, PropertyInfo property)
        {
            _cache[ordinal] = MetadataProvider<TRow>.GetUnderlyingType(property, out _) ? CompileIsDBNullBinder(instance, property) : static row => false;
        }

        /// <summary>
        /// Compiles a lambda expression that 
        /// returns the specified column value as object
        /// </summary>
        /// 
        /// <remarks>
        /// Primarily used by Microsoft.Data.SqlClient for columns with unknown types.
        /// </remarks>
        private static Delegate CompileBoxedGetValueBinder(LambdaExpression getValue)
        {
            Expression<Func<TRow, object>> lambda = Expression.Lambda
                      <Func<TRow, object>>
                      (Expression.Convert(getValue.Body, typeof(object)), getValue.Parameters);

            return lambda.Compile();
        }

        /// <summary>
        /// Compiles two lambda expressions for
        /// the specified column: one typed and
        /// one boxed.
        /// </summary>
        private static (Delegate Typed, Delegate Boxed) CompileGetValueBinder(ParameterExpression instance, PropertyInfo property)
        {
            LambdaExpression lambda = Expression.Lambda(MetadataProvider<TRow>.GetUnderlyingType(property, out Type type) && type.IsValueType ? 
                Expression.Property(
                Expression.Property(instance, property), nameof(Nullable<>.Value)) :
                Expression.Property(instance, property),
                instance);

            return (lambda.Compile(), CompileBoxedGetValueBinder(lambda));
        }

        /// <summary>
        /// Adds two lambda expressions
        /// to the cache: one typed and
        /// one boxed.
        /// </summary>
        private static void AddGetValueBinder(ParameterExpression instance, int ordinal, PropertyInfo property)
        {
            (Delegate Typed, Delegate Boxed) = CompileGetValueBinder(instance, property);

            _cache[ordinal + TRow.Metadata.Columns.Length] = Typed;
            _cache[ordinal + TRow.Metadata.Columns.Length * 2] = Boxed;
        }

        /// <summary>
        /// Returns a null-check lambda for the specified column.
        /// </summary>
        public static Func<TRow, bool> GetIsDBNullBinder(int ordinal)
        {
            return Unsafe.As<Func<TRow, bool>>(_cache[ordinal]);
        }

        /// <summary>
        /// Returns a lambda getter for the specified column.
        /// </summary>
        public static Func<TRow, T> GetValueBinder<T>(int ordinal)
        {
            return _cache[ordinal + TRow.Metadata.Columns.Length] is Func<TRow, T> direct ? direct : Unsafe.As<Func<TRow, T>>(_cache[ordinal + TRow.Metadata.Columns.Length * 2]);
        }

        static PropertyBinder()
        {
            ParameterExpression instance = Expression.Parameter(TRow.Type);

            foreach (IColumnInternalMetadata metadata in TRow.Metadata.Columns)
            {
                PropertyInfo property = TRow.Type.GetProperty(metadata.Name);

                AddIsDBNullBinder(instance, metadata.Ordinal, property);
                AddGetValueBinder(instance, metadata.Ordinal, property);
            }
        }
    }
}