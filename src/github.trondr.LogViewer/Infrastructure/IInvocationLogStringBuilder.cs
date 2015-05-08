using Castle.DynamicProxy;

namespace github.trondr.LogViewer.Infrastructure
{
    public interface IInvocationLogStringBuilder
    {
        string BuildLogString(IInvocation invocation, InvocationPhase invocationPhase);
    }

    public enum InvocationPhase
    {
        Start,
        End,
        Error
    }
}