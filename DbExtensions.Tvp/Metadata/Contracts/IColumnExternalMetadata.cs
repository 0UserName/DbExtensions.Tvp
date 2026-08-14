namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface IColumnExternalMetadata : IColumnInternalMetadata
    {
        /// <summary>
        /// Gets the maximum
        /// length of a text
        /// column.
        /// </summary>
        /// 
        /// <remarks>
        /// Ignored for non-text columns.
        /// </remarks>
        int MaxLength
        {
            get;
        }

        /// <summary>
        /// Gets a value that indicates whether the values in each row of the column must be unique.
        /// </summary>
        bool Unique
        {
            get;
        }
    }
}