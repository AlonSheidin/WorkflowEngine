using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Utility;
using WorkflowEngine.Utility.Logger;

namespace WorkflowEngine.Engine;

public class WorkflowEngine
{
    private WorkflowContext _context;
    public Process Process {get; private set;}
    
    public event Action<WorkflowEvent> WorkflowEventOccurred;

    private void RaiseEvent(WorkflowEvent evt)
    {
        WorkflowEventOccurred?.Invoke(evt);
    }

    public WorkflowEngine(string definition)
    {
        _context = new WorkflowContext();
        Process = JsonHelper.GetProcess(definition)
                  ?? throw new Exception("Process not found");
        Process.SetTasksInTaskStates();

    }

    public void Execute()
    {
        var runner = new WorkflowRunner(Process, _context);
        runner.WorkflowEventOccurred += new ConsoleLogger().OnWorkflowEvent;
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowStarted, Process.Name ?? ""));
        runner.Run(Process.States[Process.StartState]);
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowCompleted, Process.Name ?? ""));
    }

    public async Task ExecuteAsync()
    {
        var runner = new WorkflowRunner(Process, _context);
        runner.WorkflowEventOccurred += new ConsoleLogger().OnWorkflowEvent;
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowStarted, Process.Name ?? ""));
        await runner.RunAsync(Process.States[Process.StartState]);
        RaiseEvent(new WorkflowEvent(WorkflowEventType.WorkflowCompleted, Process.Name ?? ""));
    }

}

