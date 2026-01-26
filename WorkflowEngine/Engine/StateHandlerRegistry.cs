using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Tasks.Implementation;

namespace WorkflowEngine.Engine;

public static class StateHandlerRegistry
{
    private static readonly Dictionary<string, ITask> Tasks = new()
    {
        {"Start",	new StartTask()},
        {"ValidateOrder",	new ValidateOrderTask()},
        {"ChargePayment",	new ChargePaymentTask()},
        {"ShipOrder",	new ShipOrderTask()}
    };
    
    
    public static ITask GetTaskByName(string name)
    {
        return Tasks.GetValueOrDefault(name) ?? throw new InvalidOperationException($"No task with name {name} found");
    }

    public static void SetTasksInTaskStates(this Process process)
    {
        foreach (var statePair in process.States)
        {   
            statePair.Value.Name = statePair.Key;
            if(statePair.Value is TaskState taskState)
                taskState.Task = Tasks.GetValueOrDefault(statePair.Key) ?? throw new InvalidOperationException($"No task with name {statePair.Key} found");
        }
    }
}
