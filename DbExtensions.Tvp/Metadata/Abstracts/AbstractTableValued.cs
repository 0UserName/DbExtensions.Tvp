using DbExtensions.Tvp.Metadata.Contracts;

using System;

namespace DbExtensions.Tvp.Metadata.Abstracts
{
    public abstract class AbstractTableValued<TRow> : ITableValued where TRow : ITableValued
    {
        /// <inheritdoc/>
        public static Type Type
        {
            get => typeof(TRow);
        }

        /// <inheritdoc/>
        public static ITableMetadata Metadata
        {
            get => field ??= new TableMetadata<TRow>();
        }

        /// <inheritdoc/>
        public T GetValue<T>(int ordinal)
        {
            return (T)Type.GetProperty(Metadata.Columns[ordinal].Name).GetValue(this);
        }
    }
}