namespace DbExtensions.Tvp.Metadata.Contracts
{
    public interface ITableMetadata
    {
        /// <summary>
        /// The parameter type name.
        /// </summary>
        /// 
        /// <remarks>
        /// The type name must match the name of 
        /// a compatible type previously created 
        /// on the server.
        /// </remarks>
        string Name
        {
            get;
        }

        /// <summary>
        /// Describes the column metadata.
        /// </summary>
        IColumnInternalMetadata[] Columns
        {
            get;
        }
    }
}