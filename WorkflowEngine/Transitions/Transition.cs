using System.Text.Json.Serialization;

namespace WorkflowEngine.Transitions;

public record Transition(string Condition, string Next);

