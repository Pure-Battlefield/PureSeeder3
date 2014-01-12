using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PureSeeder.Core.PartialObjects
{
    public interface IBuildExpressionStack
    {
        Stack<string> BuildStack<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression);
    }

    public class BuildExpressionStack : IBuildExpressionStack
    {
        public Stack<string> BuildStack<TEntity, TValue>(Expression<Func<TEntity, TValue>> expression)
        {
            var expressionStack = new Stack<string>();

            var expr = (expression.Body) as MemberExpression;

            if (expr == null)
                return expressionStack;

            AddExpressionToStack(expressionStack, expr);

            return expressionStack;
        }

        private void AddExpressionToStack(Stack<string> stack, MemberExpression expression)
        {
            var propName = ((PropertyInfo)expression.Member).Name;

            // Push the property name onto the stack
            stack.Push(propName);

            var memExpr = expression.Expression as MemberExpression;

            // Check if we should recurse
            if (expression.Expression != null && memExpr != null)
                AddExpressionToStack(stack, (MemberExpression)expression.Expression);
        }

        public Stack BuildStack(string expressionString)
        {
            var stack = new Stack();

            if (expressionString == String.Empty)
                return stack;

            var splitExpression = expressionString.Split('.');

            for (var i = splitExpression.GetUpperBound(0); i >= 0; i--)
            {
                stack.Push(splitExpression[i]);
            }

            return stack;
        }
    }
}
