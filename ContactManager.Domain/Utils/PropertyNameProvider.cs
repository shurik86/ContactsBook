using System;
using System.Linq.Expressions;

namespace ContactsBook.Domain.Utils
{
    /// <summary>
    /// Responsible for providing property name by LINQ member access <see cref="Expression"/>.
    /// </summary>
    public static class PropertyNameProvider
    {
        /// <summary>
        /// Returns the string name of a property.
        /// </summary>
        public static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException(@"Value must be a lamda expression", "expression");
            }

            if (!(expression.Body is MemberExpression body))
            {
                throw new ArgumentException(@"The body of the expression must be a member expression", "expression");
            }

            return body.Member.Name;
        }

        /// <summary>
        /// Gets the name of the property. This overload may be used in static methods.
        /// </summary>
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new ArgumentException("Invalid expression type");

            if (expression.Body.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression.Body).Operand;
                return BuildString(operand);
            }

            if (expression.Body.NodeType == ExpressionType.MemberAccess)
                return ((MemberExpression)expression.Body).Member.Name;

            throw new ArgumentException("Invalid expression body type");
        }

        private static string BuildString(Expression propertySelector)
        {
            switch (propertySelector.NodeType)
            {
                case ExpressionType.Lambda:
                    var lambdaExpression = (LambdaExpression)propertySelector;
                    return BuildString(lambdaExpression.Body);

                case ExpressionType.Quote:
                    var unaryExpression = (UnaryExpression)propertySelector;
                    return BuildString(unaryExpression.Operand);

                case ExpressionType.MemberAccess:
                    var propertyInfo = ((MemberExpression)propertySelector).Member;
                    return propertyInfo.Name;
            }
            throw new InvalidOperationException("Expression must be a member expression or an SubInclude call: " + propertySelector);
        }
    }

}
