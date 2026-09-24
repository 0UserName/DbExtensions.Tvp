using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Rows;

using System;
using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    [TestFixture(typeof(InternalMetadataTableValued), nameof(InternalMetadataTableValued.Property0), typeof(int))]
    [TestFixture(typeof(InternalMetadataTableValued), nameof(InternalMetadataTableValued.Property1), typeof(int))]
    public sealed class NullableTypeTests<TRow>(string column, Type type) where TRow : ITableValued
    {
        [Test]
        public void TestDataReader()
        {
            _ = AssertionContext.ExecuteAsync<TRow, DbDataReader>(async (_, parameter) => Assert.That(parameter.GetSchemaTable().AsEnumerable().First(r => r.Field<string>(SchemaTableColumn.ColumnName) == column).Field<Type>(SchemaTableColumn.DataType), Is.EqualTo(type)));
        }

        [Test]
        public void TestDataTable()
        {
            _ = AssertionContext.ExecuteAsync<TRow, DataTable>(async (_, parameter) => Assert.That(parameter.Columns[column].DataType, Is.EqualTo(type)));
        }
    }
}