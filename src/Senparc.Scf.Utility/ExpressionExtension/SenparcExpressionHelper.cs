using System;
using System.Linq.Expressions;

namespace Senparc.Scf.Utility
{
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
            var where = Expression.Lambda<Func<TEntity, bool>>(ValueCompare.ExpressionBody, new[] { ValueCompare.PE });
            return where;
        }
    }


    public class ValueCompare<TEntity>
    {
        /// <summary>
        /// 统一重新绑定参数Expression.Parameter
        /// </summary>
        private sealed class ParameterRebinder : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            public ParameterRebinder(ParameterExpression parameter) => _parameter = parameter;

            protected override Expression VisitParameter(ParameterExpression p) => base.VisitParameter(_parameter);
        }

        public Expression ExpressionBody { get; set; }

        public ParameterExpression PE { get; set; }

        private ParameterRebinder _parameterRebinder;

        public ValueCompare()
        {
            PE = Expression.Parameter(typeof(TEntity), "z");
            _parameterRebinder = new ParameterRebinder(PE);
        }

        public ValueCompare<TEntity> Contains(ValueCompare<TEntity> vc)
        {
            return this;
        }

        //public ValueCompare<TEntity> Create(Expression<Func<TEntity, bool>> where)
        //{
        //    ExpressionBody = where;
        //    return this;
        //}

        public ValueCompare<TEntity> Create(Expression<Func<TEntity, bool>> filter)
        {
            var getterBody = _parameterRebinder.Visit(filter.Body);//统一参数
            ExpressionBody = getterBody;

            if (ExpressionBody == null)
            {
                throw new Exception("ExpressionBody为NULL");
            }
            return this;
        }

        public ValueCompare<TEntity> AndAlso(bool combineWhenTrue, Expression<Func<TEntity, bool>> filter)
        {
            return Combine(combineWhenTrue, ExpressionType.AndAlso, filter);
        }

        public ValueCompare<TEntity> OrElse(bool combineWhenTrue, Expression<Func<TEntity, bool>> filter)
        {
            return Combine(combineWhenTrue, ExpressionType.OrElse, filter);
        }

        public ValueCompare<TEntity> Combine(ExpressionType expressionType, Expression<Func<TEntity, bool>> filter)
        {
            if (ExpressionBody == null)
            {
                return Create(filter);
            }

            Expression left = ExpressionBody;
            Expression right = _parameterRebinder.Visit(filter.Body);//统一参数

            ExpressionBody = Expression.MakeBinary(expressionType, left, right);
            return this;
        }

        public ValueCompare<TEntity> Combine(bool combineWhenTrue, ExpressionType expressionType, Expression<Func<TEntity, bool>> filter)
        {
            if (!combineWhenTrue)
            {
                return this;
            }

            return Combine(expressionType, filter);
        }
    }
}