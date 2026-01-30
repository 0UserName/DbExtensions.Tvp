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
        /// Creates a table-valued parameter.
        /// </summary>
        public static IDisposable Build<TRow>(this IEnumerable<TRow> rows, bool dt = true) where TRow : class, ITableValued
        {
            IParameter<TRow> parameter = dt ?
                ParameterPool<ParameterDT<TRow>>.Shared.Get() :
                ParameterPool<ParameterDR<TRow>>.Shared.Get();

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

        public static IDisposable Build<TRow>(this TRow row, bool dt = false) where TRow : class, ITableValued
        {
            using (RentedBuffer<TRow> buffer = new
                   RentedBuffer<TRow>
                   (1))
            {
                buffer[0] = row;

                return buffer.Segment.Build(dt);
            }
        }
    }
}