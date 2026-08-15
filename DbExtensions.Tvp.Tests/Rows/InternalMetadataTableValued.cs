using DbExtensions.Tvp.Metadata;

using DbExtensions.Tvp.Tests.Rows.Abstracts;

namespace DbExtensions.Tvp.Tests.Rows
{
    [TableMetadata(nameof(InternalMetadataTableValued))]
    internal sealed class InternalMetadataTableValued : AbstractMetadataTableValued<InternalMetadataTableValued>
    { }
}