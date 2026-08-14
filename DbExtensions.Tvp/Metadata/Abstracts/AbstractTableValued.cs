using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Data;

using System.Runtime.CompilerServices;

namespace DbExtensions.Tvp.Metadata.Abstracts
{
    public abstract class AbstractTableValued<TRow> : ITableValued where TRow : class, ITableValued
    {
        /// <inheritdoc/>
        public static Type Type
        {
            get => typeof(TRow);
        }

        /// <inheritdoc/>
        public static ITableMetadata Metadata
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public bool IsDBNull(int ordinal)
        {
            return PropertyBinder<TRow>.GetIsDBNullBinder(ordinal)(Unsafe.As<TRow>(this)) && (TRow.Metadata.Columns[ordinal].AllowDBNull ? true : throw new ConstraintException($"Column with ordinal { ordinal } does not allow null"));
        }

        /// <inheritdoc/>
        public T GetValue<T>(int ordinal)
        {
            return PropertyBinder<TRow>.GetValueBinder<T>(ordinal)(Unsafe.As<TRow>(this));
        }

        static AbstractTableValued()
        {
            Metadata = new TableMetadata(MetadataProvider<TRow>.Get<TableMetadataAttribute>().Name, MetadataStorage.GetColumns<TRow>());
        }
    }
}