using WorkflowEngine.Engine.Context;
using WorkflowEngine.Persistence;
using WorkflowEngine.States;

using WorkflowEngine.Utility;
using WorkflowEngine.Utility.Logger;

namespace WorkflowEngine.Engine;

public class WorkflowEngine
{
    private WorkflowContext _context;
    public string DefinitionStore;
    public string WorkflowInstanceStore;
    private IWorkflowStore storage;
    public Process Process { get; private set; }
    
    public event Action<WorkflowEvent> WorkflowEventOccurred;

    private void RaiseEvent(WorkflowEvent evt) => WorkflowEventOccurred?.Invoke(evt);

    public WorkflowEngine(string definitionStore, string workflowInstanceStore)
    {
        DefinitionStore = definitionStore;
        WorkflowInstanceStore = workflowInstanceStore;
        storage = new JsonWorkflowStore(workflowInstanceStore);
    }

    public async Task RunAsync(int instanceId)
    {
        var instance = storage.LoadInstance(instanceId);
        Process = JsonHelper.GetProcess(DefinitionStore + "\\" + instance.ProcessName)
                  ?? throw new Exception($"Could not find process {instance.ProcessName}");
        Process.SetTasksInTaskStates();

        _context = instance.Context;
        
        if (instance.Status != WorkflowStatus.Running)
        {
            await ExecuteAsync(Process.States[Process.StartState], instance);
        }
        else if (instance.Status != WorkflowStatus.Completed)
        {
            await ExecuteAsync(Process.States[instance.CurrentStateName], instance);
        }
    }

    private async Task ExecuteAsync(State startState , WorkflowInstance startInstance)
    {
        var runner = new WorkflowRunner(Process, _context);
        runner.WorkflowEventOccurred += new ConsoleLogger().OnWorkflowEvent;
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowStarted, Process.Name ?? string.Empty));
        await runner.RunAsync(startState, startInstance);
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowCompleted, Process.Name ?? string.Empty));
    }

}

