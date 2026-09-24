using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    [TestFixture(typeof(ExternalMetadataTableValued), nameof(ExternalMetadataTableValued.Property6))]
    [TestFixture(typeof(ExternalMetadataTableValued), nameof(ExternalMetadataTableValued.Property7))]
    public sealed class TrimTests<TRow>(string column) where TRow : ITableValued
    {
        [Test]
        public void TestDataReader()
        {
            _ = AssertionContext.ExecuteAsync<TRow, DbDataReader>(async (_, parameter) => Assert.That(parameter.GetSchemaTable().AsEnumerable().Any(r => r.Field<string>(SchemaTableColumn.ColumnName) == column), Is.False));
        }

        [Test]
        public void TestDataTable()
        {
            _ = AssertionContext.ExecuteAsync<TRow, DataTable>(async (_, parameter) => Assert.That(parameter.Columns.Contains(column), Is.False));
        }
    }
}