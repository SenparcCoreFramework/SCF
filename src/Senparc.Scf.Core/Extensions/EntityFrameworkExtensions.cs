using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Senparc.Scf.Core.Extensions
{
    public class EntityFrameworkExtensions
    {
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }

            if (null == values)
            {
                throw new ArgumentNullException(nameof(values));
            }

            ParameterExpression p = valueSelector.Parameters.Single();

            // p => valueSelector(p) == values[0] || valueSelector(p) == ...

            if (!values.Any())
            {
                return e => false;
            }

            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));

            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
    }
}