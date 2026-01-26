using System.Runtime.InteropServices.ComTypes;
using WorkflowEngine.States;
using WorkflowEngine.Tasks;
using WorkflowEngine.Transitions;

namespace WorkflowEngine.Engine;

public class WorkflowRunner(Process process, WorkflowContext context)
{
    private Process Process { get; set; } = process;
    private State? CurrentState { get; set; }
    private TaskResult TaskResult { get; set; }
    private WorkflowContext Context { get; set; } = context;

    public bool Log { get; set; }= true;

    public void Run(State startState)
    {
        CurrentState = startState;
        while (CurrentState is not null)
        {
            if(Log)
                Console.Write(" --> state: "+CurrentState.Name);
            if (CurrentState is TaskState taskState)
            {
                TaskResult = taskState.Task.Execute(Context);
                if(Log)
                    Console.Write(", Result: "+TaskResult);
            }
            if(Log)
                Console.WriteLine();
            CurrentState = GetNextState();
        }

    }

    public async Task RunAsync(State startState)
    {
        
            CurrentState = startState;
            while (CurrentState is not null)
            {
                if (Log)
                    Console.Write(" --> state: " + CurrentState.Name);
                if (CurrentState is TaskState taskState)
                {
                    TaskResult = await taskState.Task.ExecuteAsync(Context);
                    if (Log)
                        Console.Write(", Result: " + TaskResult);
                }

                if (Log)
                    Console.WriteLine();
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
            return TaskResult == TaskResult.Success ? Process.States[state.Next] : throw new Exception("Failure of safe task");
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