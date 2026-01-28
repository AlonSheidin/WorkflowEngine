using System.Linq.Expressions;

namespace WorkflowEngine.Engine.Context.ContextEvents;

public record IncrementEvent(Expression<Func<WorkflowContext, int>> Property, int Amount) : IContextEvent;

