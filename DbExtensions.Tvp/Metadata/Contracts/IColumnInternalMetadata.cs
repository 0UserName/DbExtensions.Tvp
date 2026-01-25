using System;
using System.Data;

namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface IColumnInternalMetadata
    {
        /// <summary>
        /// The name of the column.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// The type of 
        /// data stored 
        /// in the 
        /// column.
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// The zero-based position of the column.
        /// </summary>
        int Ordinal
        {
            get;
        }

        DataColumn CreateColumn();
    }
}