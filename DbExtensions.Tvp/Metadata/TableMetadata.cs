using DbExtensions.Tvp.Metadata.Contracts;

namespace DbExtensions.Tvp.Metadata
{
    internal sealed record class TableMetadata(string Name, IColumnInternalMetadata[] Columns) : ITableMetadata
    { }
}