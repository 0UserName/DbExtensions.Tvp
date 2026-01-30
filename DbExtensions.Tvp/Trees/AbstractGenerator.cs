using DbExtensions.Tvp.Metadata.Contracts;

using System.Linq.Expressions;

namespace DbExtensions.Tvp.Trees
{
    internal abstract class AbstractGenerator<TRow> where TRow : ITableValued
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static LambdaExpression CreateGetter(string propertyName)
        {
            ParameterExpression param = Expression.Parameter(TRow.Type, "x");

            MemberExpression property = Expression.Property(param, propertyName);

            return Expression.Lambda(property, param);
        }
    }
}
