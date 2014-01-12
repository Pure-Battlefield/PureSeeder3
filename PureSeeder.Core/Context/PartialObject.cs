using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            CreateEntity(json);
        }

        public static PartialObject<TEntity> Create(string json)
        {
            return new PartialObject<TEntity>(new BuildExpressionStack(), json);
        }

        private void CreatePartialInfo(string json)
        {
            PartialInfo = JObject.Parse(json);
        }

        private void CreateEntity(string json)
        {
            Entity = JsonConvert.DeserializeObject<TEntity>(json, new JsonSerializerSettings() {ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor});
        }

        public JObject PartialInfo { get; private set; }
        public TEntity Entity { get; private set; }

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

        public void MergeValue<TValue>(Expression<Func<TEntity, TValue>> propertyExpr, TEntity mergeTarget)
        {
            if (!Exists(propertyExpr))  // Check if the property was passed in
                return;

            var memberExpr = propertyExpr.Body as MemberExpression;
            if (memberExpr == null)
                return;
            var parameterExpr = memberExpr.Expression as ParameterExpression;
            var valueParameterExpr = Expression.Parameter(typeof (TValue));

            var valueLambda = propertyExpr.Compile();

            var inputValue = valueLambda(Entity);
            var mergeTargetValue = valueLambda(mergeTarget);  // Testcode

            var assignLambda = Expression.Lambda<Action<TEntity, TValue>>(
               Expression.Assign(propertyExpr.Body, valueParameterExpr),
               parameterExpr,
               valueParameterExpr
               ).Compile();

            assignLambda(mergeTarget, inputValue);
            mergeTargetValue = valueLambda(mergeTarget); // Testcode
        }
    }
}
