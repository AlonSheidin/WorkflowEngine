using System.ComponentModel.DataAnnotations;

namespace WorkflowEngine.States;

public class Process
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int? Version { get; set; }
    public required string StartState { get; set; }
    public Dictionary<string,State> States { get; set; }

    public override string ToString()
    {
        string states = "";
        foreach (var state in States)
        {
            states += $"\n{state.Key}: {state.Value}";
        }
        return $"Id: {Id}, Name: {Name}, Version: {Version},  StartState: {StartState}, States {states}";
    }
}