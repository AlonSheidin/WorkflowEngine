using System.Runtime.InteropServices.ComTypes;
using WorkflowEngine.Tasks;

namespace WorkflowEngine.Engine;

public class WorkflowRunner
{
    public Process Process { get; set; }
    public State CurrentState { get; set; }
    public TaskResult TaskResult { get; set; }
    
    public State? GetNextState()
    {
        switch (CurrentState)
        {
            case TaskState state:
                return GetNextStateOfTaskState(state);
            case DecisionState decisionState:
                return GetNextStateOfDecisionState(decisionState);
            case EndState:
                return null;
            default:
                throw new ArgumentException("Unknown state", nameof(CurrentState));
        }
    }

    private State GetNextStateOfTaskState(TaskState state)
    {
        if (state.Next != null && state is { OnSuccess: null, OnFailure: null, RetryPolicy: null })
        {
            return TaskResult == TaskResult.Success ? Process.States[state.Next] : throw new Exception("Failure of safe task");
        }

        if (state.OnSuccess != null && state.OnFailure != null && state is { RetryPolicy: not null, Next: null })
        {
            if (state.CurrentRetryCount < state.RetryPolicy.MaxRetries)
            {
                state.CurrentRetryCount++;
                return state;
            }
            return Process.States[state.OnFailure];
        }
        
        throw new Exception($"Task State cannot have a Next state and OnSuccess or OnFailure Sate's (state: {state})");
    }

    private State GetNextStateOfDecisionState(DecisionState decisionState)
    {
        throw new NotImplementedException();
    }
}