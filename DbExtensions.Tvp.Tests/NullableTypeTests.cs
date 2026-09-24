using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System;
using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    [TestFixture(typeof(InternalMetadataTableValued), nameof(InternalMetadataTableValued.Property0), typeof(int))]
    [TestFixture(typeof(InternalMetadataTableValued), nameof(InternalMetadataTableValued.Property1), typeof(int))]
    public sealed class NullableTypeTests<TRow>(string column, Type type) : AbstractTests where TRow : ITableValued
    {
        [Test]
        public void TestDataReader()
        {
            ThatAsync<Type, TRow, DbDataReader>(async (_, parameter) => parameter.GetSchemaTable().AsEnumerable().First(r => r.Field<string>(SchemaTableColumn.ColumnName) == column).Field<Type>(SchemaTableColumn.DataType), Is.EqualTo(type)).Wait();
        }

        [Test]
        public void TestDataTable()
        {
            ThatAsync<Type, TRow, DataTable>(async (_, parameter) => parameter.Columns[column].DataType, Is.EqualTo(type)).Wait();
        }
    }
}