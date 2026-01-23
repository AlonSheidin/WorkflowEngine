namespace WorkflowEngine;

public class TaskState : State
{
    public string Next { get; set; }
    
    public string OnSuccess { get; set; }
    public string OnFailure { get; set; }
    public RetryPolicy? RetryPolicy { get; private set; }

    public TaskState() : base(StateType.Task, "")
    {
        
    }

    public override string ToString()
    {
        return base.ToString()+
            $" Next: {Next?.ToString() ?? "null"}"+
               $", OnSuccess:{OnSuccess?.ToString() ?? "null"}" +
               $", OnFailure:{OnFailure?.ToString() ?? "null"} ";
    }
}