using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Contracts.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System;
using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    public sealed class TypeTests : AbstractTests
    {
        [TestCase(nameof(InternalMetadataTableValued.Property0), typeof(int), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property0) } is of type { nameof(Int32) }", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(nameof(InternalMetadataTableValued.Property1), typeof(int), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property1) } is of type { nameof(Int32) }", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        public void TestDataReaderNullableType<TRow>(string column, Type type) where TRow : ITableValued
        {
            ThatAsync<Type, TRow, DbDataReader>(async (_, parameter) => parameter.GetSchemaTable().AsEnumerable().First(r => r.Field<string>(SchemaTableColumn.ColumnName) == column).Field<Type>(SchemaTableColumn.DataType), Is.EqualTo(type)).Wait();
        }

        [TestCase(nameof(InternalMetadataTableValued.Property0), typeof(int), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property0) } is of type { nameof(Int32) }", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(nameof(InternalMetadataTableValued.Property1), typeof(int), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property1) } is of type { nameof(Int32) }", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        public void TestDataTableNullableType<TRow>(string column, Type type) where TRow : ITableValued
        {
            ThatAsync<Type, TRow, DataTable>(async (_, parameter) => parameter.Columns[column].DataType, Is.EqualTo(type)).Wait();
        }
    }
}