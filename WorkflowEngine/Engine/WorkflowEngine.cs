using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Utility;

namespace WorkflowEngine.Engine;

public class WorkflowEngine
{
    private WorkflowContext _context;
    private WorkflowRunner _runner;
    private Process process;

    public WorkflowEngine(string definition)
    {
        _context = new WorkflowContext();
        process = JsonHelper.GetProcess(definition)
                  ?? throw new Exception("Process not found");
        process.SetTasksInTaskStates();

    }

    public void Execute()
    {
        Console.WriteLine("--- Starting Process ---");
        State startState = GetStateByName(process.StartState);
        var startTask = StateHandlerRegistry.GetTaskByName(process.StartState);
        _runner = new WorkflowRunner
        {
            CurrentState = startState,
            Process = process,
            TaskResult = startTask.Execute(_context),
            Context = _context
        };
        _runner.Run();
        Console.WriteLine("--- Process Finished ---");
    }

    public State GetStateByName(string name)
    {
        return process.States[name];
    }

}

