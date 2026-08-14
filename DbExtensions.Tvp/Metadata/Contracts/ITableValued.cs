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
        /// Return whether the
        /// specified field is
        /// set to null.
        /// </summary>
        bool IsDBNull(int ordinal);

        /// <summary>
        /// Returns the value of the specified field.
        /// </summary>
        T GetValue<T>(int ordinal);
    }
}