using System.Text.Json.Serialization;
using WorkflowEngine.Transitions;

namespace WorkflowEngine.States;
using WorkflowEngine.Engine;


public class DecisionState : State
{
    public Transition[] Transitions { get; set; }
    [JsonIgnore]
    public Func<WorkflowEngine, bool> Condition { get; set; }
    public DecisionState() : base(StateType.Decision, "description")
    {
        
    }

    public override string ToString()
    {
        return base.ToString() + string.Join(", ", Transitions.Select(t => t.ToString()));
    }
}