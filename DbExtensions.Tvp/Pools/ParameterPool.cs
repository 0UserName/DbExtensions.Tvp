using Microsoft.Extensions.ObjectPool;

using System.Threading;

namespace DbExtensions.Tvp.Pools
{
    internal sealed class ParameterPool<TParameter> : DefaultObjectPool<TParameter> where TParameter : class, IResettable
    {
        public static readonly
            ThreadLocal<ParameterPool<TParameter>> Shared = new
            ThreadLocal<ParameterPool<TParameter>>
            (
                () => new ParameterPool<TParameter>()
            );

        private ParameterPool() : base(new ParameterObjectPolicy<TParameter>())
        { }
    }
}