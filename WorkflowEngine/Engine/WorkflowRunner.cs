using System.Runtime.InteropServices.ComTypes;
using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Transitions;
using WorkflowEngine.Utility.Logger;

namespace WorkflowEngine.Engine;

public class WorkflowRunner(Process process, WorkflowContext context)
{
    private Process Process { get; set; } = process;
    private State? CurrentState { get; set; }
    private TaskResult TaskResult { get; set; }
    private WorkflowContext Context { get; set; } = context;
    
    public event Action<WorkflowEvent> WorkflowEventOccurred;
    private void RaiseEvent(WorkflowEvent evt)
    {
        WorkflowEventOccurred?.Invoke(evt);
    }
    
    public void Run(State startState)
    {
        CurrentState = startState;
        while (CurrentState is not null)
        {
            
            if (CurrentState is TaskState taskState)
            {
                TaskResult = taskState.Task.Execute(Context);
            }
            CurrentState = GetNextState();
        }

    }

    public async Task RunAsync(State startState)
    {
        
            CurrentState = startState;
            while (CurrentState is not null)
            {
                RaiseEvent(new WorkflowEvent(WorkflowEventType.StateEntered, Process.Name, CurrentState.Name));
                if (CurrentState is TaskState taskState)
                {
                    RaiseEvent(new  WorkflowEvent(WorkflowEventType.TaskStarted, Process.Name, taskState.Name));
                    
                    TaskResult = await taskState.Task.ExecuteAsync(Context);
                    
                    RaiseEvent(TaskResult is TaskResult.Success
                        ? new WorkflowEvent(WorkflowEventType.TaskCompleted, Process.Name, taskState.Name)
                        : new WorkflowEvent(WorkflowEventType.TaskFailed, Process.Name, taskState.Name));
                }
                RaiseEvent(new WorkflowEvent(WorkflowEventType.StateExited, Process.Name, CurrentState.Name));
                CurrentState = GetNextState();
                
            }
    }

    private State? GetNextState()
    {
        return CurrentState switch
        {
            TaskState state => GetNextStateOfTaskState(state),
            DecisionState decisionState => GetNextStateOfDecisionState(decisionState),
            EndState => null,
            _ => throw new ArgumentException("Unknown state", nameof(CurrentState))
        };
    }

    private State GetNextStateOfTaskState(TaskState state)
    {
        if (state.Next != null && state is { OnSuccess: null, OnFailure: null, RetryPolicy: (0, 0) })
        {
            return TaskResult == TaskResult.Success ? Process.States[state.Next] : throw new Exception("Failure of safe task, none safe tasks should have a onSuccess and OnFailure Fields");
        }

        if (state.OnSuccess != null && state.OnFailure != null && state.RetryPolicy is not null &&  state.Next is null )
        {
            if(TaskResult == TaskResult.Success)
                return Process.States[state.OnSuccess];
            
            if (state.CurrentRetryCount >= state.RetryPolicy.MaxRetries && TaskResult == TaskResult.Failure)
            {
                return Process.States[state.OnFailure];
            }
            
            state.CurrentRetryCount++;
            return state;
        }
        
        throw new Exception($"Task State cannot have a Next state and OnSuccess or OnFailure Sate's (state: {state})");
    }

    private State GetNextStateOfDecisionState(DecisionState decisionState)
    {
        foreach (var transition in decisionState.Transitions)
        {
            var condition = transition.CompileCondition();
            if (condition(Context))
            {
                return Process.States[transition.Next];
            }
        }
        throw new Exception("No Next state found");
    }
}