using DbExtensions.Tvp.Metadata;
using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Parameters;

using DbExtensions.Tvp.Tests.Rows;

using System;
using System.Collections.Generic;

using System.Data.Common;

using System.Reflection;

using System.Threading.Tasks;

namespace DbExtensions.Tvp.Tests
{
    public static class AssertionContext
    {
        /// <summary>
        /// Creates data rows, builds a parameter from
        /// them, and executes the specified assertion
        /// using both.
        /// </summary>
        public static async Task ExecuteAsync<TRow, TParameter>(Func<IEnumerable<TRow>, TParameter, Task> assertion) where TRow : ITableValued
        {
            IEnumerable<TRow> rows = RowsFactory.Create<TRow>();

            using (IDisposable parameter = rows.Build(typeof(TParameter).IsAssignableTo(typeof(DbDataReader))))
            {
                await assertion(rows, (TParameter)parameter);
            }
        }

        static AssertionContext()
        {
            MetadataStorage.AddColumns(typeof(ExternalMetadataTableValued).GetCustomAttribute<TableMetadataAttribute>().Name, new IColumnExternalMetadata[]
            {
                new ColumnExternalMetadata(default, true , nameof(ExternalMetadataTableValued.Property0), typeof(int)   , 5, -1, false),
                new ColumnExternalMetadata(default, true , nameof(ExternalMetadataTableValued.Property1), typeof(int)   , 4, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property2), typeof(int)   , 3, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property3), typeof(int)   , 2, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property4), typeof(string), 1, -1, false),
                new ColumnExternalMetadata(default, false, nameof(ExternalMetadataTableValued.Property5), typeof(string), 0, -1, false)
            });
        }
    }
}