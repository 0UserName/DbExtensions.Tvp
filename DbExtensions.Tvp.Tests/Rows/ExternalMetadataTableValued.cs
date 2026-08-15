using DbExtensions.Tvp.Metadata;

using DbExtensions.Tvp.Tests.Rows.Abstracts;

namespace DbExtensions.Tvp.Tests.Rows
{
    [TableMetadata(nameof(ExternalMetadataTableValued))]
    internal sealed class ExternalMetadataTableValued : AbstractMetadataTableValued<ExternalMetadataTableValued>
    {
        public string Property4
        {
            get;
            set;
        }

        public string Property5
        {
            get;
            set;
        }

        public string Property6
        {
            get;
            set;
        }

        public string Property7
        {
            get;
            set;
        }
    }
}