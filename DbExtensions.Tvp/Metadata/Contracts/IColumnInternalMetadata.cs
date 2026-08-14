using System;

namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface IColumnInternalMetadata
    {
        /// <summary>
        /// Gets a value that indicates whether null values
        /// are allowed in this column for rows that belong
        /// to the table.
        /// </summary>
        bool AllowDBNull
        {
            get;
        }

        /// <summary>
        /// Gets the name
        /// of the column.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the type of data stored in the column.
        /// </summary>
        Type Type
        {
            get;
        }

        /// <summary>
        /// Gets the (zero-based) position of the column.
        /// </summary>
        int Ordinal
        {
            get;
        }
    }
}