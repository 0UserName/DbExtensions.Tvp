using DbExtensions.Tvp.Metadata.Contracts;

using Microsoft.Extensions.ObjectPool;

using System;
using System.Collections.Generic;

namespace DbExtensions.Tvp.Parameters.Contracts
{
    internal interface IParameter<TRow> : IResettable, IDisposable where TRow : ITableValued
    {
        void Load(IEnumerable<TRow> rows);
    }
}