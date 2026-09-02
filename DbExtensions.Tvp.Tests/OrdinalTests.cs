using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Data;
using System.Data.Common;

using System.Linq;

namespace DbExtensions.Tvp.Tests
{
    public sealed class OrdinalTests : AbstractTests
    {
        [TestCase(0, nameof(InternalMetadataTableValued.Property0), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property0) } is defined in the class layout at ordinal [0]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(1, nameof(InternalMetadataTableValued.Property1), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property1) } is defined in the class layout at ordinal [1]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(2, nameof(InternalMetadataTableValued.Property2), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property2) } is defined in the class layout at ordinal [2]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(3, nameof(InternalMetadataTableValued.Property3), TestName = $"DbDataReader: { nameof(InternalMetadataTableValued.Property3) } is defined in the class layout at ordinal [3]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(0, nameof(ExternalMetadataTableValued.Property5), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property5) } is defined in the external metadata at ordinal [0]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(1, nameof(ExternalMetadataTableValued.Property4), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property4) } is defined in the external metadata at ordinal [1]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(2, nameof(ExternalMetadataTableValued.Property3), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property3) } is defined in the external metadata at ordinal [2]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(3, nameof(ExternalMetadataTableValued.Property2), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property2) } is defined in the external metadata at ordinal [3]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(4, nameof(ExternalMetadataTableValued.Property1), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property1) } is defined in the external metadata at ordinal [4]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(5, nameof(ExternalMetadataTableValued.Property0), TestName = $"DbDataReader: { nameof(ExternalMetadataTableValued.Property0) } is defined in the external metadata at ordinal [5]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataReaderOrdinal<TRow>(int ordinal, string column) where TRow : ITableValued
        {
            That<string, TRow, DbDataReader>((_, p) => p.GetSchemaTable().AsEnumerable().First(r => r.Field<int>(SchemaTableColumn.ColumnOrdinal) == ordinal).Field<string>(SchemaTableColumn.ColumnName), Is.EqualTo(column));
        }

        [TestCase(0, nameof(InternalMetadataTableValued.Property0), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property0) } is defined in the class layout at ordinal [0]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(1, nameof(InternalMetadataTableValued.Property1), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property1) } is defined in the class layout at ordinal [1]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(2, nameof(InternalMetadataTableValued.Property2), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property2) } is defined in the class layout at ordinal [2]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(3, nameof(InternalMetadataTableValued.Property3), TestName = $"DataTable: { nameof(InternalMetadataTableValued.Property3) } is defined in the class layout at ordinal [3]", TypeArgs = new[] { typeof(InternalMetadataTableValued) })]
        [TestCase(0, nameof(ExternalMetadataTableValued.Property5), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property5) } is defined in the external metadata at ordinal [0]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(1, nameof(ExternalMetadataTableValued.Property4), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property4) } is defined in thr external metadata at ordinal [1]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(2, nameof(ExternalMetadataTableValued.Property3), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property3) } is defined in the external metadata at ordinal [2]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(3, nameof(ExternalMetadataTableValued.Property2), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property2) } is defined in the external metadata at ordinal [3]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(4, nameof(ExternalMetadataTableValued.Property1), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property1) } is defined in the external metadata at ordinal [4]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        [TestCase(5, nameof(ExternalMetadataTableValued.Property0), TestName = $"DataTable: { nameof(ExternalMetadataTableValued.Property0) } is defined in the external metadata at ordinal [5]", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataTableOrdinal<TRow>(int ordinal, string column) where TRow : ITableValued
        {
            That<string, TRow, DataTable>((_, p) => p.Columns[ordinal].ColumnName, Is.EqualTo(column));
        }
    }
}