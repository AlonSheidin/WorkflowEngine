using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Tasks.Implementation;

namespace WorkflowEngine.Engine;

public static class StateHandlerRegistry
{

    private static Dictionary<string, ITask> GetTasksFromAssembly()
    {
        var dict = new Dictionary<string, ITask>();
        var tasks = typeof(ITask).Assembly.GetTypes()
            .Where(t => t.IsClass && typeof(ITask).IsAssignableFrom(t) && !t.IsInterface)
            .Select(t => (ITask) Activator.CreateInstance(t)!);

        foreach (var task in tasks)
        {
            dict.Add(task.GetType().Name.Replace("Task", ""), task);
        }

        return dict;
    }
    
    
    public static void SetTasksInTaskStates(this Process process)
    {
        var tasks = GetTasksFromAssembly();
        foreach (var statePair in process.States)
        {   
            statePair.Value.Name = statePair.Key;
            if(statePair.Value is TaskState taskState)
                taskState.Task = tasks.GetValueOrDefault(statePair.Key) ??
                                 throw new InvalidOperationException($"No task with name {statePair.Key} found");
        }
    }
}
