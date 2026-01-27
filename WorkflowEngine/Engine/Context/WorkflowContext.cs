using System.Linq.Expressions;
using WorkflowEngine.Engine.Context.ContextEvents;

namespace WorkflowEngine.Engine.Context;
public class WorkflowContext(
    int version = 0,
    string orderStatus = "",
    int retryCount = 0,
    bool paymentApproved = false)
{
    public int Version { get; } = version;

    public string OrderStatus { get; } = orderStatus;
    public int RetryCount { get; } = retryCount;
    public bool PaymentApproved { get; } = paymentApproved;

    public WorkflowContext Apply(IContextEvent ev)
    {
        return ev switch
        {
            SetEvent<string> e when Is(e.Property, x => x.OrderStatus)
                => With(orderStatus: e.Value),

            SetEvent<bool> e when Is(e.Property, x => x.PaymentApproved)
                => With(paymentApproved: e.Value),

            IncrementEvent e when Is(e.Property, x => x.RetryCount)
                => With(retryCount: RetryCount + e.Amount),

            _ => throw new InvalidOperationException("Unsupported event")
        };
    }

    public WorkflowContext With(
        string? orderStatus = null,
        int? retryCount = null,
        bool? paymentApproved = null)
    {
        return new WorkflowContext(
            version: Version + 1,
            orderStatus: orderStatus ?? OrderStatus,
            retryCount: retryCount ?? RetryCount,
            paymentApproved: paymentApproved ?? PaymentApproved
        );
    }

    private static bool Is<T>(
        Expression<Func<WorkflowContext, T>> a,
        Expression<Func<WorkflowContext, T>> b)
    {
        return ((MemberExpression)a.Body).Member.Name ==
               ((MemberExpression)b.Body).Member.Name;
    }
}

