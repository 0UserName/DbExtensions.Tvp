using DbExtensions.Tvp.Metadata.Contracts;
using DbExtensions.Tvp.Metadata.Contracts.Abstracts;

namespace DbExtensions.Tvp.Tests.Rows.Abstracts
{
    public abstract class AbstractMetadataTableValued<TRow> : AbstractTableValued<TRow> where TRow : class, ITableValued
    {
        public int? Property0
        {
            get;
            set;
        }

        public int? Property1
        {
            get;
            set;
        }

        public int Property2
        {
            get;
            set;
        }

        public int Property3
        {
            get;
            set;
        }
    }
}