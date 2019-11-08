using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Senparc.Scf.Utility
{
    public class SenparcExpressionModifier<TEntity> : ExpressionVisitor
    {
        public BinaryExpression BE { get; set; }

        public SenparcExpressionModifier()
        { }

        public ReadOnlyCollection<Expression> AndAlso(IList<Expression> expressions)
        {
            ReadOnlyCollection<Expression> col = new ReadOnlyCollection<Expression>(expressions);
            var result = Visit(col);
            return result;
        }

        public Expression<Func<TEntity, bool>> BuildWhereExpression()
        {
            ParameterExpression pe = Expression.Parameter(typeof(string), "z");

            Expression<Func<TEntity, bool>> where = Expression.Lambda<Func<TEntity, bool>>(BE, pe);
            return where;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            //if (b.NodeType == ExpressionType.AndAlso)
            //{
            //    Expression left = this.Visit(b.Left);
            //    Expression right = this.Visit(b.Right);

            //    // Make this binary expression an OrElse operation instead of an AndAlso operation.
            //    return Expression.MakeBinary(ExpressionType.OrElse, left, right, b.IsLiftedToNull, b.Method);
            //}

            if (b.IsLifted)
            {
                return base.VisitBinary(b);
            }
            if (BE == null)
            {
                BE = b;
            }
            else
            {
                //默认为添加And
                BE = Expression.MakeBinary(ExpressionType.AndAlso, BE, b);
            }

            return base.VisitBinary(b);
        }
    }
}