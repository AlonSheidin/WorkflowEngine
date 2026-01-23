namespace WorkflowEngine;

public class DecisionState : State
{
    public Transition[] Transitions { get; private set; }
    public DecisionState() : base(StateType.Decision, "description")
    {
        
    }
}