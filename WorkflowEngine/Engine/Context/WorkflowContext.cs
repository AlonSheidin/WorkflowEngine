using System.Linq.Expressions;
using WorkflowEngine.Engine.Context.ContextEvents;
namespace WorkflowEngine.Engine.Context;
public class WorkflowContext
{
    public string OrderStatus { get; }
    public bool PaymentApproved { get; }
    public int RetryCount { get; }

    public WorkflowContext(string orderStatus = "", bool paymentApproved = false, int retryCount = 0)
    {
        OrderStatus = orderStatus;
        PaymentApproved = paymentApproved;
        RetryCount = retryCount;
    }

    public WorkflowContext With(
        string? orderStatus = null,
        bool? paymentApproved = null,
        int? retryCount = null)
    {
        return new WorkflowContext(
            orderStatus ?? OrderStatus,
            paymentApproved ?? PaymentApproved,
            retryCount ?? RetryCount
        );
    }

    public WorkflowContext Apply(IContextEvent ev) =>
        ev switch
        {
            SetEvent<string> e when PropertyIs(e.Property, x => x.OrderStatus) =>
                With(orderStatus: e.Value),
            SetEvent<bool> e when PropertyIs(e.Property, x => x.PaymentApproved) =>
                With(paymentApproved: e.Value),
            IncrementEvent e when PropertyIs(e.Property, x => x.RetryCount) =>
                With(retryCount: RetryCount + e.Amount),
            _ => throw new InvalidOperationException("Unknown event")
        };

    private static bool PropertyIs<T>(Expression<Func<WorkflowContext, T>> a,
        Expression<Func<WorkflowContext, T>> b)
    {
        return ((MemberExpression)a.Body).Member.Name == ((MemberExpression)b.Body).Member.Name;
    }

    public override string ToString()
    {
        return $"OrderStatus:{OrderStatus} PaymentApproved:{OrderStatus} RetryCount:{OrderStatus}";
    }
}