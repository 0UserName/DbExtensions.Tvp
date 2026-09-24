using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    [TestFixture(typeof(ExternalMetadataTableValued), nameof(ExternalMetadataTableValued.Property6))]
    [TestFixture(typeof(ExternalMetadataTableValued), nameof(ExternalMetadataTableValued.Property7))]
    public sealed class TrimTests<TRow>(string column) : AbstractTests where TRow : ITableValued
    {
        [Test]
        public void TestDataReader()
        {
            ThatAsync<bool, TRow, DbDataReader>(async (_, parameter) => parameter.GetSchemaTable().AsEnumerable().Any(r => r.Field<string>(SchemaTableColumn.ColumnName) == column), Is.False).Wait();
        }

        [Test]
        public void TestDataTable()
        {
            ThatAsync<bool, TRow, DataTable>(async (_, parameter) => parameter.Columns.Contains(column), Is.False).Wait();
        }
    }
}