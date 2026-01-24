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
        _runner = new WorkflowRunner(process, _context);
        _runner.Run(process.States[process.StartState]);
        Console.WriteLine("--- Process Finished ---");
    }

    

}

