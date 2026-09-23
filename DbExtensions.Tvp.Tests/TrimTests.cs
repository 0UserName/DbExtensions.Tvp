using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    public sealed class TrimTests : AbstractTests
    {
        [TestCase(nameof(ExternalMetadataTableValued.Property6), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property6) } is ignored", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(nameof(ExternalMetadataTableValued.Property7), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property7) } is ignored", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataReaderTrimColumn<TRow>(string column) where TRow : ITableValued
        {
            ThatAsync<bool, TRow, DbDataReader>(async (_, parameter) => parameter.GetSchemaTable().AsEnumerable().Any(r => r.Field<string>(SchemaTableColumn.ColumnName) == column), Is.False).Wait();
        }

        [TestCase(nameof(ExternalMetadataTableValued.Property6), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property6) } is ignored", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(nameof(ExternalMetadataTableValued.Property7), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property7) } is ignored", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataTableTrimColumn<TRow>(string column) where TRow : ITableValued
        {
            ThatAsync<bool, TRow, DataTable>(async (_, parameter) => parameter.Columns.Contains(column), Is.False).Wait();
        }
    }
}