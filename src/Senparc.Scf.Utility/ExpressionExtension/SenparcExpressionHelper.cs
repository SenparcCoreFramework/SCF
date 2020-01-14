using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Senparc.Scf.Utility
{
    /// <summary>
    /// 支持关联表查询
    /// 
    /// 类型如：
    ///   var seh = new SenparcExpressionHelper<T>();
    ///   seh.ValueCompare
    ///       .AndAlso(true, i => !i.IsDeleted && i.MPAccountId == MPAccount.Id)
    ///       .AndAlso(!string.IsNullOrEmpty(SearchText), i => i.MPKeywordList.Count(c => c.Content.Contains(SearchText)) > 0);
    ///   var where = seh.BuildWhereExpression();
    ///   
    /// 其中关联表的Count统计，在SenparcExpressionHelper下，会吧变量c当作主表属性而报错
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class SenparcExpressionHelper<TEntity> where TEntity : class
    {
        public ValueCompare<TEntity> ValueCompare { get; set; }
        public SenparcExpressionHelper()
        {
            ValueCompare = new ValueCompare<TEntity>();
        }

        /// <summary>
        /// 生成where表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> BuildWhereExpression()
        {
            if (ValueCompare.ExpressionBody == null)
            {
                //不需要查询
                Expression<Func<TEntity, bool>> returnTrue = z => true;
                return returnTrue;
            }
            var where = ValueCompare.ExpressionBody;
            return where;
        }
    }
    public class ValueCompare<TEntity>
    {
        public Expression<Func<TEntity, bool>> ExpressionBody { get; set; }
        public ValueCompare()
        {
        }
        /// <summary>
        /// Combines the first predicate with the second using the logical "and".
        /// </summary>
        public ValueCompare<TEntity> AndAlso(bool combineWhenTrue, Expression<Func<TEntity, bool>> filter)
        {
            return Combine(combineWhenTrue, filter, Expression.AndAlso);
        }
        /// <summary>
        /// Combines the first predicate with the second using the logical "or".
        /// </summary>
        public ValueCompare<TEntity> OrElse(bool combineWhenTrue, Expression<Func<TEntity, bool>> filter)
        {
            return Combine(combineWhenTrue, filter, Expression.OrElse);
        }

        /// <summary>
        /// Negates the predicate.
        /// </summary>
        public ValueCompare<TEntity> Not()
        {
            var negated = Expression.Not(ExpressionBody.Body);
            ExpressionBody = Expression.Lambda<Func<TEntity, bool>>(negated, ExpressionBody.Parameters);
            return this;
        }
        public ValueCompare<TEntity> Combine(Expression<Func<TEntity, bool>> filter, Func<Expression, Expression, Expression> merge)
        {
            if (ExpressionBody == null)
            {
                ExpressionBody = PredicateBuilder.Create(filter);
                return this;
            }
            ExpressionBody = ExpressionBody.Compose(filter, merge);

            return this;
        }
        public ValueCompare<TEntity> Combine(bool combineWhenTrue, Expression<Func<TEntity, bool>> filter, Func<Expression, Expression, Expression> merge)
        {
            if (!combineWhenTrue)
            {
                return this;
            }
            return Combine(filter, merge);
        }
    }
    public static class PredicateBuilder
    {
        /// <summary>
        /// Creates a predicate expression from the specified lambda expression.
        /// </summary>
        public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate) { return predicate; }

        /// <summary>
        /// Combines the first expression with the second using the specified merge function.
        /// </summary>
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // zip parameters (map from parameters of second to parameters of first)
            var map = first.Parameters
                .Select((f, i) => new { f, s = second.Parameters[i] })
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with the parameters in the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // create a merged lambda expression with parameters from the first expression
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        /// <summary>
        /// ParameterRebinder
        /// </summary>
        class ParameterRebinder : ExpressionVisitor
        {
            /// <summary>
            /// The ParameterExpression map
            /// </summary>
            readonly Dictionary<ParameterExpression, ParameterExpression> map;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
            /// </summary>
            /// <param name="map">The map.</param>
            ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
            {
                this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            /// <summary>
            /// Replaces the parameters.
            /// </summary>
            /// <param name="map">The map.</param>
            /// <param name="exp">The exp.</param>
            /// <returns>Expression</returns>
            public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
            {
                return new ParameterRebinder(map).Visit(exp);
            }

            /// <summary>
            /// Visits the parameter.
            /// </summary>
            /// <param name="p">The p.</param>
            /// <returns>Expression</returns>
            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression replacement;

                if (map.TryGetValue(p, out replacement))
                {
                    p = replacement;
                }

                return base.VisitParameter(p);
            }
        }
    }
}
