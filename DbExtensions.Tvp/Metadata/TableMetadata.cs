using DbExtensions.Tvp.Metadata.Contracts;

namespace DbExtensions.Tvp.Metadata
{
    internal sealed class TableMetadata<TRow> : ITableMetadata where TRow : ITableValued
    {
        /// <inheritdoc/>
        public string Name
        {
            get => field ??= MetadataProvider<TRow>.Get<TableMetadataAttribute>().Name;
        }

        /// <inheritdoc/>
        public IColumnInternalMetadata[] Columns
        {
            get => field ??= MetadataStorage.GetColumns<TRow>();
        }
    }
}