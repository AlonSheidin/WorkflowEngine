using WorkflowEngine.Utility;

namespace WorkflowEngine;
using Engine;
class Program
{
    static void Main(string[] args)
    {
        var engine = new WorkflowEngine("F:\\Coding\\WorkflowEngine\\WorkflowEngine\\Definitions\\base-process.json");
        engine.Execute();
        
    }
    
    /*
     * /WorkflowEngine
        │
        ├── /Engine
        │   ├── WorkflowEngine.cs           // Core engine orchestrator
        │   ├── WorkflowRunner.cs           // Handles executing instances
        │   ├── WorkflowContext.cs          // Tracks workflow instance data
        │   ├── StateHandlerRegistry.cs     // Maps state names to functions
        │   └── ResultTypes/                // Types returned by tasks
        │       └── TaskResult.cs
        │
        ├── /States
        │   ├── State.cs  +                  // Abstract base class
        │   ├── TaskState.cs +
        │   ├── DecisionState.cs +
        │   ├── WaitState.cs
        │   ├── EndState.cs
        │   └── RetryPolicy.cs    +          // Record or class
        │
        ├── /Transitions
        │   ├── Transition.cs      +         // Represents a transition (condition + next)
        │   └── ConditionEvaluator.cs       // Evaluates string expressions against workflow context
        │
        ├── /Tasks
        │   ├── ITaskHandler.cs             // Interface for task functions
        │   ├── StartTask.cs
        │   ├── ExecuteMainTask.cs
        │   └── OtherTaskImplementations.cs
        │
        ├── /Definitions
        │   └── Workflows/                  // JSON workflow files
        │       └── BaseWorkflow.json
        │
        ├── /Utilities
        │   ├── JsonLoader.cs               // Load workflow definitions
        │   ├── Logger.cs                   // Logging engine events
        │   └── ValidationHelper.cs         // JSON/schema validation
        │
        └── Program.cs                       // Entry point for testing/running engine

    */
}