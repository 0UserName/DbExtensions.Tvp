using DbExtensions.Tvp.Binders;

using System;
using System.Data;

using System.Runtime.CompilerServices;

namespace DbExtensions.Tvp.Metadata.Contracts.Abstracts
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
        public T GetFieldValue<T>(int ordinal)
        {
            return PropertyBinder<TRow>.GetFieldValue<T>(Unsafe.As<TRow>(this), ordinal);
        }

        /// <inheritdoc/>
        public object[] GetValues(object[] values)
        {
            return ArrayBinder<TRow>.GetValues(Unsafe.As<TRow>(this), values);
        }

        /// <inheritdoc/>
        public bool IsDBNull(int ordinal)
        {
            return PropertyBinder<TRow>.IsDBNull(Unsafe.As<TRow>(this), ordinal) && (TRow.Metadata.Columns[ordinal].AllowDBNull ? true : throw new ConstraintException($"Column with ordinal { ordinal } does not allow null"));
        }

        static AbstractTableValued()
        {
            Metadata = new TableMetadata(MetadataProvider<TRow>.Get<TableMetadataAttribute>().Name, MetadataStorage.GetColumns<TRow>());
        }
    }
}