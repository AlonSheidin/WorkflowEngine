using WorkflowEngine.Engine.Context;
using WorkflowEngine.States;

namespace WorkflowEngine.Persistence;

public class WorkflowInstance
{
    public int InstanceId { get; set; }
    public string ProcessName { get; set; }
    public string CurrentStateName { get; set; }
    public WorkflowContext Context { get; set; }
    public WorkflowStatus Status { get; set; }

    public WorkflowInstance(int instanceId, string processName,  string currentStateName, WorkflowContext context, WorkflowStatus status)
    {
        InstanceId = instanceId;
        ProcessName = processName;
        CurrentStateName = currentStateName;
        Context = context;
        Status = status;
    }
    public WorkflowInstance()
    {
        
    }

    public override string ToString()
    {
        return $"InstanceId: {InstanceId}, CurrentStateName: {CurrentStateName}, Context: [{Context}], Status: {Status}";
    }
}