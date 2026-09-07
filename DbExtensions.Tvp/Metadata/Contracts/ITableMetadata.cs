namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface ITableMetadata
    {
        /// <summary>
        /// Gets the parameter type name.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets the column metadata.
        /// </summary>
        IColumnInternalMetadata[] Columns
        {
            get;
        }
    }
}