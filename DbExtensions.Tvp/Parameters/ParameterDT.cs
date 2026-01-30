using DbExtensions.Tvp.Metadata.Contracts;
using DbExtensions.Tvp.Parameters.Contracts;
using DbExtensions.Tvp.Trees;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;

using System.Data;

namespace DbExtensions.Tvp.Parameters
{
    internal sealed class ParameterDT<TRow> : DataTable, IParameter<TRow> where TRow : ITableValued
    {
        private readonly ObjectPool<ParameterDT<TRow>> _pool;

        private readonly static Action<TRow, DataRow> initFunc = DataRowGenerators<TRow>.GetOrCreate();

        /// <inheritdoc/>
        public void Load(IEnumerable<TRow> rows)
        {
            BeginLoadData();

            foreach (TRow row in rows)
            {
                DataRow dr = NewRow();
                dr.BeginEdit();
                initFunc(row, dr);
                dr.EndEdit();
                Rows.Add(dr);
            }

            EndLoadData();
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

        public ParameterDT(ObjectPool<ParameterDT<TRow>> pool) : base(TRow.Metadata.Name)
        {
            _pool = pool;

            BeginInit();

            foreach (IColumnInternalMetadata metadata in TRow.Metadata.Columns)
            {
                Columns.Add(metadata.CreateColumn());
            }

            EndInit();
        }
    }
}