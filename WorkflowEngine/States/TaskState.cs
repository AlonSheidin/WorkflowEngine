namespace WorkflowEngine;

public class TaskState : State
{
    public string Next { get; set; }
    public string OnSuccess { get; set; }
    public string OnFailure { get; set; }
    public RetryPolicy? RetryPolicy { get; set; }
    public int CurrentRetryCount { get; set; } = 0;
    public TaskState() : base(StateType.Task, "")
    {
        
    }

    public override string ToString()
    {
        return base.ToString()+
            $" Next: {Next?.ToString()}"+
               $", OnSuccess:{OnSuccess?.ToString()}" +
               $", OnFailure:{OnFailure?.ToString()} "+
            $" RetryPolicy:{RetryPolicy?.ToString()}";
    }
}