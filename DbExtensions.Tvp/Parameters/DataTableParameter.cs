using DbExtensions.Tvp.Metadata.Contracts;

using DbExtensions.Tvp.Parameters.Contracts;

using Microsoft.Extensions.ObjectPool;

using System.Collections.Generic;

using System.Data;

namespace DbExtensions.Tvp.Parameters
{
    internal sealed class DataTableParameter<TRow> : DataTable, IParameter<TRow> where TRow : ITableValued
    {
        private readonly ObjectPool<DataTableParameter<TRow>> _pool;

        /// <inheritdoc/>
        public void Load(IEnumerable<TRow> rows)
        {
            object[] buffer = new object[TRow.Metadata.Columns.Length];

            foreach (TRow row in rows)
            {
                DataRowBinder<TRow>.Get()(this, row, buffer);
            }
        }

        /// <inheritdoc/>
        public bool TryReset()
        {
            Clear();

            return true;
        }

        /// <inheritdoc/>
        public new void Dispose()
        {
            _pool.Return(this);
        }

        public DataTableParameter() : base(TRow.Metadata.Name)
        {
            this.InitColumns<TRow>();
        }

        public DataTableParameter(ObjectPool<DataTableParameter<TRow>> pool) : this()
        {
            _pool = pool;
        }
    }
}