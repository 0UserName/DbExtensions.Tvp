using DbExtensions.Tvp.Metadata.Contracts;

using System;
using System.Collections.Generic;

using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DbExtensions.Tvp.Trees
{
    internal sealed class DataRowGenerators<TRow> : AbstractGenerator<TRow> where TRow : ITableValued
    {
        public static readonly
            DataRowGenerators<TRow> Instance = new
            DataRowGenerators<TRow>
            ();

        /// <summary>
        /// 
        /// </summary>
        public static Action<TRow, DataRow> GetOrCreate()
        {
            List<Expression> expressions = new
            List<Expression>
           ();

            List<ParameterExpression> vars = new
            List<ParameterExpression>
           ();

            ParameterExpression inArgTRow = Expression.Variable(TRow.Type, "x");
            ParameterExpression inArgDRow = Expression.Variable(typeof(DataRow), "dr");

            int i = 0;
            foreach (PropertyInfo property in TRow.Type.GetProperties())
            {

                /*
                 * $value = .Invoke (.Lambda #Lambda1<System.Func`2[TVP,System.Boolean]>)($x)
                 * 
                 * .Lambda #Lambda1<System.Func`2[TVP,System.Boolean]>(TVP $x) {
                 * 
                 *     $x.Property1
                 * }
                 */
                ParameterExpression getterValue = Expression.Variable(property.PropertyType, $"value_{ property.Name }");
                LambdaExpression getter = CreateGetter(property.Name);
                InvocationExpression invokeGetter = Expression.Invoke(getter, inArgTRow);
                BinaryExpression assignResultToGetterValue = Expression.Assign(getterValue, invokeGetter);

                var methods = typeof(DataRowExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static);


                    MethodInfo setFieldMethod  = methods
                    .Where(m => m.Name == "SetField")
                    .First(m => m.GetParameters().Any(p => p.Name == "columnName")).MakeGenericMethod(property.PropertyType);


                MethodCallExpression call= Expression.Call(setFieldMethod, inArgDRow, Expression.Constant(TRow.Metadata.Columns[i].Name), getterValue);


                vars.Add(getterValue);
                expressions.Add(assignResultToGetterValue);
                expressions.Add(call);

                i++;
            }

           // Собираем блок: сначала присваивание, потом использование
           var block = Expression.Block(
               vars,
               expressions
           );

            Expression<Action<TRow, DataRow>> lambda = Expression.Lambda<Action<TRow, DataRow>>(block, inArgTRow, inArgDRow);

            Action<TRow, DataRow> func = lambda.Compile();
            return func;

        }

    }
}
