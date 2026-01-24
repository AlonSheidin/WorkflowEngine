using System.Linq.Expressions;
using System.Reflection;
using WorkflowEngine.Engine;

namespace WorkflowEngine.Transitions;

public static class ConditionEvaluator
{

    
    public static Func<WorkflowContext, bool> CompileCondition(this Transition transition)
    {
        ParameterExpression contextParameter =
            Expression.Parameter(typeof(WorkflowContext), "context");

        Expression body =
            ParseAndExpression(transition.Condition, contextParameter);

        Expression<Func<WorkflowContext, bool>> lambda =
            Expression.Lambda<Func<WorkflowContext, bool>>(
                body,
                contextParameter
            );

        return lambda.Compile();
    }
    
    private static Expression ParseAndExpression(
        string text,
        ParameterExpression contextParameter)
    {
        string[] parts =
            text.Split("&&", StringSplitOptions.TrimEntries);

        Expression current =
            ParseComparison(parts[0], contextParameter);

        for (int i = 1; i < parts.Length; i++)
        {
            Expression right =
                ParseComparison(parts[i], contextParameter);

            current =
                Expression.AndAlso(current, right);
        }

        return current;
    }
    private static Expression ParseComparison(
        string text,
        ParameterExpression contextParameter)
    {
        if (text.Contains("=="))
            return BuildBinary(text, "==", contextParameter);

        if (text.Contains("<"))
            return BuildBinary(text, "<", contextParameter);

        if (text.Contains(">"))
            return BuildBinary(text, ">", contextParameter);

        throw new InvalidOperationException(
            $"Unsupported comparison: {text}"
        );
    }
    private static Expression BuildBinary(
        string text,
        string op,
        ParameterExpression contextParameter)
    {
        string[] parts =
            text.Split(op, StringSplitOptions.TrimEntries);

        string propertyName = parts[0];
        string literalText = parts[1];

        PropertyInfo property =
            ResolveProperty(propertyName);

        MemberExpression left =
            Expression.Property(contextParameter, property);

        object literalValue =
            ConvertLiteral(literalText, property.PropertyType);

        ConstantExpression right =
            Expression.Constant(literalValue, property.PropertyType);

        return op switch
        {
            "==" => Expression.Equal(left, right),
            "<"  => Expression.LessThan(left, right),
            ">"  => Expression.GreaterThan(left, right),
            _ => throw new InvalidOperationException()
        };
    }
    private static PropertyInfo ResolveProperty(string propertyName)
        => typeof(WorkflowContext).GetProperty(propertyName) 
           ?? throw new InvalidOperationException($"Unknown context property '{propertyName}'");

    
    private static object ConvertLiteral(
        string text,
        Type targetType)
    {
        if (targetType == typeof(bool))
            return bool.Parse(text);

        if (targetType == typeof(int))
            return int.Parse(text);

        if (targetType == typeof(string))
            return text.Trim('"');

        throw new InvalidOperationException(
            $"Unsupported literal type '{targetType.Name}'"
        );
    }
}


