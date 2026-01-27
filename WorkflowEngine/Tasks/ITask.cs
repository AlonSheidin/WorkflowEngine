using WorkflowEngine.Engine;
using WorkflowEngine.Engine.Context;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Tasks;

public interface ITask
{
    Task<(TaskResult Result, List<IContextEvent> Events)> ExecuteAsync(
        WorkflowContext context);
}
