
using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Transitions;
using WorkflowEngine.Utility.Logger;

namespace WorkflowEngine.Engine;

public class WorkflowRunner(Process process, WorkflowContext context)
{
    private readonly Process _process = process;
    private State? _currentState;
    private TaskResult _taskResult;
    private readonly WorkflowContext _context = context;
    
    public event Action<WorkflowEvent>? WorkflowEventOccurred;
    private void RaiseEvent(WorkflowEvent evt) => WorkflowEventOccurred?.Invoke(evt);
    
    public void Run(State startState)
    {
        _currentState = startState;
        while (_currentState is not null)
        {
            if (_currentState is TaskState taskState)
            {
                _taskResult = taskState.Task.Execute(_context);
            }
            _currentState = GetNextState();
        }
    }

    public async Task RunAsync(State startState)
    {
        _currentState = startState;
        while (_currentState is not null)
        {
            RaiseEvent(new WorkflowEvent(WorkflowEventType.StateEntered, _process.Name ?? string.Empty, _currentState.Name));
            if (_currentState is TaskState taskState)
            {
                RaiseEvent(new WorkflowEvent(WorkflowEventType.TaskStarted, _process.Name ?? string.Empty, taskState.Name));
                _taskResult = await taskState.Task.ExecuteAsync(_context);
                RaiseEvent(_taskResult is TaskResult.Success
                    ? new WorkflowEvent(WorkflowEventType.TaskCompleted, _process.Name ?? string.Empty, taskState.Name)
                    : new WorkflowEvent(WorkflowEventType.TaskFailed, _process.Name ?? string.Empty, taskState.Name));
            }
            RaiseEvent(new WorkflowEvent(WorkflowEventType.StateExited, _process.Name ?? string.Empty, _currentState.Name));
            _currentState = GetNextState();
        }
    }

    private State? GetNextState()
    {
        return _currentState switch
        {
            TaskState state => GetNextStateOfTaskState(state),
            DecisionState decisionState => GetNextStateOfDecisionState(decisionState),
            EndState => null,
            _ => throw new ArgumentException("Unknown state", nameof(_currentState))
        };
    }

    private State GetNextStateOfTaskState(TaskState state)
    {
        if (state.Next != null && state is { OnSuccess: null, OnFailure: null, RetryPolicy: (0, 0) })
        {
            return _taskResult == TaskResult.Success ? _process.States[state.Next] : throw new Exception("Failure of safe task, non-safe tasks should have OnSuccess and OnFailure fields");
        }

        if (state.OnSuccess != null && state.OnFailure != null && state.RetryPolicy is not null && state.Next is null)
        {
            if (_taskResult == TaskResult.Success)
                return _process.States[state.OnSuccess];
            
            if (state.CurrentRetryCount >= state.RetryPolicy.MaxRetries && _taskResult == TaskResult.Failure)
            {
                return _process.States[state.OnFailure];
            }
            
            state.CurrentRetryCount++;
            return state;
        }
        
        throw new Exception($"Task State cannot have a Next state and OnSuccess or OnFailure States (state: {state})");
    }

    private State GetNextStateOfDecisionState(DecisionState decisionState)
    {
        foreach (var transition in decisionState.Transitions)
        {
            var condition = transition.CompileCondition();
            if (condition(_context))
            {
                return _process.States[transition.Next];
            }
        }
        throw new Exception("No Next state found");
    }
}