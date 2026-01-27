using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Engine.Context;

public static class ContextMerger
{
    public static WorkflowContext Merge(
        WorkflowContext context,
        IEnumerable<IContextEvent> events)
    {
        foreach (var ev in events)
        {
            context = context.Apply(ev);
        }

        return context;
    }
}
