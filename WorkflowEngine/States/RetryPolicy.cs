namespace WorkflowEngine;

public record RetryPolicy(int MaxRetries, int DelaySeconds)
{
    public override string ToString()
        => $"RetryPolicy({MaxRetries}, {DelaySeconds})";
}

