using System;

namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface ITableValued
    {
        static abstract Type Type
        {
            get;
        }

        static abstract ITableMetadata Metadata
        {
            get;
        }

        /// <summary>
        /// Gets the value of the specified column as the requested type.
        /// </summary>
        T GetFieldValue<T>(int ordinal);

        /// <summary>
        /// Populates an array of objects
        /// with the column values of the
        /// current row.
        /// </summary>
        object[] GetValues(object[] values);

        /// <summary>
        /// Gets a value that
        /// indicates whether
        /// the column is set
        /// to null.
        /// </summary>
        bool IsDBNull(int ordinal);
    }
}