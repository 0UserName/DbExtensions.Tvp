using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Parameters.Contracts;

using DbExtensions.Tvp.Pools;

using System;
using System.Collections.Generic;

namespace DbExtensions.Tvp.Parameters
{
    public static class ParameterExtensions
    {
        /// <summary>
        /// Creates a table-valued parameter containing the specified rows.
        /// </summary>
        public static IDisposable Build<TRow>(this IEnumerable<TRow> rows, bool useDataReader = true) where TRow : ITableValued
        {
            IParameter<TRow> parameter = useDataReader ? ParameterPool<DataReaderParameter<TRow>>.Shared.Value.Get() : ParameterPool<DataTableParameter<TRow>>.Shared.Value.Get();

            try
            {
                parameter.Load(rows);
            }
            catch
            {
                parameter.Dispose();

                throw;
            }

            return parameter;
        }

        public static IDisposable Build<TRow>(this TRow row, bool useDataReader = true) where TRow : ITableValued
        {
            return new TRow[] { row }.Build(useDataReader);
        }
    }
}