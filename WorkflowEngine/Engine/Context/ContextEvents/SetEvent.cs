using System.Linq.Expressions;

namespace WorkflowEngine.Engine.Context.ContextEvents;

public record SetEvent<TValue>(
    Expression<Func<WorkflowContext, TValue>> Property,
    TValue Value) : IContextEvent;

