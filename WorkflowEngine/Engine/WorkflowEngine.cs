namespace WorkflowEngine.Engine;

public class WorkflowEngine
{
    WorkflowContext _context;
    public WorkflowEngine(string definition)
    {
        _context = new WorkflowContext();
        

    }

    public void Execute()
    {
        Console.WriteLine("--- Process Started ---");
        Console.WriteLine("--- Process Finished ---");
    }
}