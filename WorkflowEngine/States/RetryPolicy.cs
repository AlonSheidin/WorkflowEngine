namespace WorkflowEngine;

public record RetryPolicy(int MaxRetries, int DelaySeconds);
