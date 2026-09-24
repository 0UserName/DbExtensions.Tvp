using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    [TestFixture(typeof(InternalMetadataTableValued), 0, nameof(InternalMetadataTableValued.Property0))]
    [TestFixture(typeof(InternalMetadataTableValued), 1, nameof(InternalMetadataTableValued.Property1))]
    [TestFixture(typeof(InternalMetadataTableValued), 2, nameof(InternalMetadataTableValued.Property2))]
    [TestFixture(typeof(InternalMetadataTableValued), 3, nameof(InternalMetadataTableValued.Property3))]
    [TestFixture(typeof(ExternalMetadataTableValued), 0, nameof(ExternalMetadataTableValued.Property5))]
    [TestFixture(typeof(ExternalMetadataTableValued), 1, nameof(ExternalMetadataTableValued.Property4))]
    [TestFixture(typeof(ExternalMetadataTableValued), 2, nameof(ExternalMetadataTableValued.Property3))]
    [TestFixture(typeof(ExternalMetadataTableValued), 3, nameof(ExternalMetadataTableValued.Property2))]
    [TestFixture(typeof(ExternalMetadataTableValued), 4, nameof(ExternalMetadataTableValued.Property1))]
    [TestFixture(typeof(ExternalMetadataTableValued), 5, nameof(ExternalMetadataTableValued.Property0))]
    public sealed class OrdinalTests<TRow>(int ordinal, string column) : AbstractTests where TRow : ITableValued
    {
        [Test]
        public void TestDataReader()
        {
            ThatAsync<string, TRow, DbDataReader>(async (_, parameter) => parameter.GetSchemaTable().AsEnumerable().First(r => r.Field<int>(SchemaTableColumn.ColumnOrdinal) == ordinal).Field<string>(SchemaTableColumn.ColumnName), Is.EqualTo(column)).Wait();
        }

        [Test]
        public void TestDataTable()
        {
            ThatAsync<string, TRow, DataTable>(async (_, parameter) => parameter.Columns[ordinal].ColumnName, Is.EqualTo(column)).Wait();
        }
    }
}