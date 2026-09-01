using Microsoft.Extensions.ObjectPool;

namespace DbExtensions.Tvp.Pools
{
    internal sealed class ParameterPool<TParameter> : DefaultObjectPool<TParameter> where TParameter : class, IResettable
    {
        public static readonly
            ParameterPool<TParameter> Shared = new
            ParameterPool<TParameter>
            ();

        private ParameterPool() : base(new ParameterObjectPolicy<TParameter>())
        { }
    }
}