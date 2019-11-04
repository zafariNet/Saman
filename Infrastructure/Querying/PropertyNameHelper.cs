using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Infrastructure.Querying
{
    public static class PropertyNameHelper
    {
        public static string ResolvePropertyName<T>(Expression<Func<T, Object>> expression)
        {
            var expr = expression.Body as MemberExpression;
            if (expr == null)
            {
                var u = expression.Body as UnaryExpression;
                expr = u.Operand as MemberExpression;
            }
            return expr.ToString().Substring(expr.ToString().IndexOf('.') + 1);
        }
    }
}
