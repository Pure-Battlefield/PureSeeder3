using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureSeeder.Core.Annotations;
using PureSeeder.Core.PartialObjects;

namespace PureSeeder.Core.Context
{
    public class PartialObject<TEntity>
    {
        private readonly IBuildExpressionStack _expressionStackBuilder;

        private PartialObject([NotNull] IBuildExpressionStack expressionStackBuilder, string json)
        {
            if (expressionStackBuilder == null) throw new ArgumentNullException("expressionStackBuilder");
            _expressionStackBuilder = expressionStackBuilder;

            CreatePartialInfo(json);
        }

        public static PartialObject<TEntity> Create(string json)
        {
            return new PartialObject<TEntity>(new BuildExpressionStack(), json);
        }

        private void CreatePartialInfo(string json)
        {
            PartialInfo = JObject.Parse(json);
        }

        public JObject PartialInfo { get; private set; }

        public bool Exists<TValue>(Expression<Func<TEntity, TValue>> expression)
        {
            var expressionStack = _expressionStackBuilder.BuildStack(expression);

            if (expressionStack.Count == 0)
                return true;

            return Exists(expressionStack, PartialInfo);
        }

        private static bool Exists(Stack<string> expressionStack, JObject jObject)
        {
            var value = jObject.GetValue(expressionStack.Pop(), StringComparison.InvariantCultureIgnoreCase);

            if (value == null)
                return false;

            if (expressionStack.Count == 0)
                return true;

            var newJObject = value as JObject;
            if (newJObject == null)
                return false; // Should probably throw an exception here but too lazy to handle it

            return Exists(expressionStack, newJObject);
        }

        public bool TryGetMatchingValue<TValue>(Expression<Func<TEntity, TValue>> expression, ref TValue valueItem)
        {
            var expressionStack = _expressionStackBuilder.BuildStack(expression);

            if (expressionStack.Count == 0)
            {
                return false;
            }

            return GetMatchingValue(expressionStack, PartialInfo, ref valueItem);
        }

        private static bool GetMatchingValue<TValue>(Stack<string> expressionStack, JObject jObject, ref TValue valueItem)
        {
            var value = jObject.GetValue(expressionStack.Pop(), StringComparison.InvariantCultureIgnoreCase);

            if (value == null)
                return false;

            if (expressionStack.Count == 0)
            {
                if (value is JArray)
                {
                    valueItem = ((JArray) value).ToObject<TValue>();
                    return true;
                }
                valueItem = value.Value<TValue>();
                return true;
            }

            var newJObject = value as JObject;
            if (newJObject == null)
                throw new ArgumentException();

            return GetMatchingValue(expressionStack, newJObject, ref valueItem);
        }

        public bool MergeItem<TValue>(Expression<Func<TEntity, TValue>> propertyExpr, TEntity mergeTarget)
        {
            if (!Exists(propertyExpr))
                return false;

            var memberExpr = propertyExpr.Body as MemberExpression;
            if (memberExpr == null)
                return false;
            var parameterExpr = memberExpr.Expression as ParameterExpression;
            var valueParameterExpr = Expression.Parameter(typeof(TValue));

            var valueLambda = propertyExpr.Compile();

            var mergeTargetValue = valueLambda(mergeTarget); // Testcode
            var newValue = default(TValue);
            var matchingValue = TryGetMatchingValue(propertyExpr, ref newValue);

            var assignLambda = Expression.Lambda<Action<TEntity, TValue>>(
               Expression.Assign(propertyExpr.Body, valueParameterExpr),
               parameterExpr,
               valueParameterExpr
               ).Compile();

            assignLambda(mergeTarget, newValue);

            mergeTargetValue = valueLambda(mergeTarget);  // Testcode
            return matchingValue;
        }

        public bool MergeItem<TValue>(Expression<Func<TEntity, TValue>> propertyExpr, ref TValue mergeTarget)
        {
            if (!Exists(propertyExpr))
                return false;

            var memberExpr = propertyExpr.Body as MemberExpression;
            if (memberExpr == null)
                return false;
            var parameterExpr = memberExpr.Expression as ParameterExpression;
            var valueParameterExpr = Expression.Parameter(typeof(TValue));

            var valueLambda = propertyExpr.Compile();

            var newValue = default(TValue);
            var matchingValue = TryGetMatchingValue(propertyExpr, ref newValue);

            mergeTarget = newValue;

            return matchingValue;
        }
    }
}
