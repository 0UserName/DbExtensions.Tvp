using DbExtensions.Tvp.Metadata.Contracts;

namespace DbExtensions.Tvp.Metadata
{
    internal sealed class TableMetadata(string name, IColumnInternalMetadata[] columns) : ITableMetadata
    {
        /// <inheritdoc/>
        public string Name
        {
            get => name;
        }

        /// <inheritdoc/>
        public IColumnInternalMetadata[] Columns
        {
            get => columns;
        }
    }
}