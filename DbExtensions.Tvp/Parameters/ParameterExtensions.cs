using DbExtensions.Tvp.Buffers;

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
        /// Creates a table-valued parameter containing the passed rows.
        /// </summary>
        /// 
        /// <returns>
        /// IDisposable that is either DbDataReader or
        /// DataTable, depending on the specified flag.
        /// </returns>
        public static IDisposable Build<TRow>(this IEnumerable<TRow> rows, bool useDataReader = true) where TRow : ITableValued
        {
            IParameter<TRow> parameter = useDataReader ? ParameterPool<DataReaderParameter<TRow>>.Shared.Get() : ParameterPool<DataTableParameter<TRow>>.Shared.Get();

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
            using (RentedBuffer<TRow> buffer = new
                   RentedBuffer<TRow>
                   (1))
            {
                buffer[0] = row;

                return buffer.Segment.Build(useDataReader);
            }
        }
    }
}