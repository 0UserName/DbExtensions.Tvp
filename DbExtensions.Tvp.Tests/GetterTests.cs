using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Tests.Abstracts;

using DbExtensions.Tvp.Tests.Rows;

using System.Collections.Generic;

using System.Data;
using System.Data.Common;

namespace DbExtensions.Tvp.Tests
{
    public sealed class GetterTests : AbstractTests
    {
        /// <remarks>
        /// It's enough to check calls of IsDbNull, GetValue, and GetFieldValue to
        /// verify TVP build correctness. Other methods like GetInt or GetLong use
        /// these internally.
        /// </remarks>
        private static string Materialize(DbDataReader reader)
        {
            List<object> values = new
            List<object>
            ();

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    values.Add(reader.IsDBNull(i) ? "null" : reader.GetValue(i));
                }
            }

            return string.Join(", ", values);
        }

        [TestCase("Value3, Value1, 6, 4, 2, 0, Value4, Value2, 6, 5, null, null", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataReaderGetter<TRow>(string expected) where TRow : ITableValued
        {
            That<bool, TRow, DbDataReader>((rows, p) => expected == Materialize(p), Is.True);
        }

        [TestCase("Value3, Value1, 6, 4, 2, 0, Value4, Value2, 6, 5, null, null", TypeArgs = new[] { typeof(ExternalMetadataTableValued) })]
        public void TestDataTableGetter<TRow>(string expected) where TRow : ITableValued
        {
            That<bool, TRow, DataTable>((rows, p) => expected == Materialize(new DataTableReader(p)), Is.True);
        }
    }
}