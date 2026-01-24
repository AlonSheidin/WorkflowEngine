using WorkflowEngine.States;
using WorkflowEngine.Tasks;

namespace WorkflowEngine.Engine;

public static class StateHandlerRegistry
{
    private static readonly Dictionary<string, ITask> Tasks = new()
    {
        { "Start", new StartTask() },
        {"End", new EndTask() },
        {"Fail", new FailTask() },
        {"Reject", new RejectTask() },
        {"ExecuteMainTask", new ExecuteMainTask() },
        {"Complete", new CompleteTask()}
    };
    
    
    public static ITask GetTaskByName(string name)
    {
        return Tasks.GetValueOrDefault(name) ?? throw new InvalidOperationException($"No task with name {name} found");
    }

    public static void SetTasksInTaskStates(this Process process)
    {
        foreach (var statePair in process.States)
        {
            if(statePair.Value is TaskState taskState)
                taskState.Task = Tasks.GetValueOrDefault(statePair.Key) ?? throw new InvalidOperationException($"No task with name {statePair.Key} found");
        }
    }
}

public class CompleteTask : ITask
{
    public TaskResult Execute(WorkflowContext context)
    {
        Console.WriteLine("Task complete");
        return TaskResult.Success;
    }
}