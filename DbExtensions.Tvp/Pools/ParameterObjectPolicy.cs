using Microsoft.Extensions.ObjectPool;

using System;

namespace DbExtensions.Tvp.Pools
{
    internal sealed class ParameterObjectPolicy<TParameter> : IPooledObjectPolicy<TParameter> where TParameter : class, IResettable
    {
        /// <inheritdoc/>
        public TParameter Create()
        {
            return (TParameter)Activator.CreateInstance(typeof(TParameter), ParameterPool<TParameter>.Shared);
        }

        /// <inheritdoc/>
        public bool Return(TParameter obj)
        {
            return obj.TryReset();
        }
    }
}