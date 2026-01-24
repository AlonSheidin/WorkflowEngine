namespace WorkflowEngine.Engine;

public class WorkflowContext
{
    public bool IsValid { get; set; }
    public int Counter { get; set; }

    public override string ToString()
    {
        return $"IsValid: {IsValid}, Counter: {Counter}";
    }
}