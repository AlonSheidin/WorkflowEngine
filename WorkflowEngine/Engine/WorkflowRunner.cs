
using WorkflowEngine.Engine.Context;
using WorkflowEngine.Persistence;
using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Transitions;
using WorkflowEngine.Utility.Logger;

namespace WorkflowEngine.Engine;

public class WorkflowRunner(Process process, WorkflowContext context)
{
    private State? _currentState;
    private TaskResult _taskResult;
    private WorkflowContext _context = context;

    public event Action<WorkflowEvent>? WorkflowEventOccurred;
    private void RaiseEvent(WorkflowEvent evt) => WorkflowEventOccurred?.Invoke(evt);
    
    

    public async Task RunAsync(State startState, WorkflowInstance instance)
    {
        _currentState = startState;
        while (_currentState is not null)
        {
            instance.CurrentStateName = _currentState.Name;
            instance.Context = _context;
            instance.Status =  WorkflowStatus.Running;
            new JsonWorkflowStore().UpdateInstance(instance);
            
            RaiseEvent(new WorkflowEvent(WorkflowEventType.StateEntered, process.Name ?? string.Empty, _currentState.Name));
            
            switch (_currentState)
            {   
                case TaskState taskState:
                {
                    RaiseEvent(new WorkflowEvent(WorkflowEventType.TaskStarted, process.Name ?? string.Empty, taskState.Name));
                
                    var taskStateExecution = await taskState.Task.ExecuteAsync(_context);
                    _taskResult = taskStateExecution.Result;
                    if(_taskResult == TaskResult.Success)
                        _context = ContextMerger.Merge(_context, taskStateExecution.Events);
                    RaiseEvent(_taskResult is TaskResult.Success
                        ? new WorkflowEvent(WorkflowEventType.TaskCompleted, process.Name ?? string.Empty, taskState.Name)
                        : new WorkflowEvent(WorkflowEventType.TaskFailed, process.Name ?? string.Empty, taskState.Name));
                    break;
                }
                case ParallelState parallelState:
                    var tasks = parallelState.Branches
                        .Select(t => process.States[t] as TaskState)
                        .OfType<TaskState>();

                   
                    
                    var parallelStateExecution = await Task.WhenAll(tasks.Select(t => t.Task.ExecuteAsync(_context)));
                    _taskResult = parallelStateExecution.All(exec => exec.Result is TaskResult.Success)
                        ? TaskResult.Success : TaskResult.Failure;
                    
                   
                    
                    if (_taskResult == TaskResult.Success)
                    {
                        var events = parallelStateExecution
                            .Select(exec => exec.Events)
                            .SelectMany(e => e);
                        
                        _context = ContextMerger.Merge(_context, events);
                    }
                    break;
            }
            
            RaiseEvent(new WorkflowEvent(WorkflowEventType.StateExited, process.Name ?? string.Empty, _currentState.Name));
            _currentState = GetNextState();
            
            
        }

        instance.Status = WorkflowStatus.Completed;
        new JsonWorkflowStore().UpdateInstance(instance);
    }

    private State? GetNextState()
    {
        return _currentState switch
        {
            TaskState state => GetNextStateOfTaskState(state),
            DecisionState decisionState => GetNextStateOfDecisionState(decisionState),
            ParallelState parallelState => GetNextStateOfParallelState(parallelState),
            EndState => null,
            _ => throw new ArgumentException("Unknown state", nameof(_currentState))
        };
    }

    private State GetNextStateOfParallelState(ParallelState state)
    {
        if (state.Next != null && state is { OnSuccess: null, OnFailure: null })
        {
            return _taskResult == TaskResult.Success
                ? process.States[state.Next] : throw new Exception("Failure of safe parallel state, non-safe parallel states should have OnSuccess and OnFailure fields");
        }

        if (state is { OnSuccess: not null, OnFailure: not null, Next: null }) 
        {
            return _taskResult == TaskResult.Success ? process.States[state.OnSuccess] : process.States[state.OnFailure];
        }
        
        throw new Exception($"Parallel State cannot have a Next state and OnSuccess or OnFailure States (state: {state})");
 
    }

    private State GetNextStateOfTaskState(TaskState state)
    {
        if (state.Next != null && state is { OnSuccess: null, OnFailure: null, RetryPolicy: (0, 0) })
        {
            return _taskResult == TaskResult.Success ? process.States[state.Next] : throw new Exception("Failure of safe task, non-safe tasks should have OnSuccess and OnFailure fields");
        }

        if (state.OnSuccess != null && state.OnFailure != null && state.RetryPolicy is not null && state.Next is null)
        {
            if (_taskResult == TaskResult.Success)
                return process.States[state.OnSuccess];
            
            if (state.CurrentRetryCount >= state.RetryPolicy.MaxRetries && _taskResult == TaskResult.Failure)
            {
                return process.States[state.OnFailure];
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
                return process.States[transition.Next];
            }
        }
        throw new Exception("No Next state found");
    }
}