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
        /// Gets the value located 
        /// at the specified index.
        /// </summary>
        T GetValue<T>(int ordinal);
    }
}