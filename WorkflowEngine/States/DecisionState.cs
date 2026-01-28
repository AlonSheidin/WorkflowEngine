using System.Text.Json.Serialization;
using WorkflowEngine.Transitions;

namespace WorkflowEngine.States;
using WorkflowEngine.Engine;


public class DecisionState() : State(StateType.Decision, "description")
{
    public Transition[] Transitions { get; set; }
    [JsonIgnore]
    public Func<WorkflowEngine, bool> Condition { get; set; }

    public override string ToString()
    {
        return base.ToString() + string.Join(", ", Transitions.Select(t => t.ToString()));
    }
}