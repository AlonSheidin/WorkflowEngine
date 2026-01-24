using System.Text.Json.Serialization;
using WorkflowEngine.Tasks;

namespace WorkflowEngine.States;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TaskState), "task")]
[JsonDerivedType(typeof(DecisionState), "decision")]
[JsonDerivedType(typeof(EndState), "end")]
public abstract class State
{
    public StateType Type { get; set; }
    public string Description { get; set; }
    
    

    protected State(StateType type, string description)
    {
        Type = type;
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public override string ToString()
    {
        return $"Type: {Type}, Description: {Description},";
    }
}

public enum StateType
{
    Task,
    Decision,
    Wait,
    End
}