namespace WorkflowEngine.Engine;

public class WorkflowContext
{
    public bool IsValid { get; set; }
    public int Counter { get; set; }
    public int Price {get; set; }
    public bool Initialized {get; set; }
    public int OrderId {get; set; }
    public bool PaymentCharged {get; set; }
    public bool Shipped {get; set; }
}