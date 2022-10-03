using GreenPipes;
using GreenPipes.Internals.Extensions;
using Stories.Abstractions;

namespace Stories.Activities;

/// <summary>
/// Catches an exception of a specific type and compensates using the behavior
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TException"></typeparam>
public class CatchFaultActivity<TInstance, TException> :
    IActivity<TInstance>
    where TInstance : class
    where TException : Exception
{
    readonly IBehavior<TInstance> _behavior;

    public CatchFaultActivity(IBehavior<TInstance> behavior)
    {
        _behavior = behavior;
    }

    void IVisitable.Accept(IStateMachineVisitor visitor)
    {
        visitor.Visit(this, x => _behavior.Accept(visitor));
    }

    public void Probe(ProbeContext context)
    {
        var scope = context.CreateScope("catch");

        scope.Add("exceptionType", TypeCache<TException>.ShortName);

        _behavior.Probe(scope.CreateScope("behavior"));
    }

    Task IActivity<TInstance>.Execute(IBehaviorContext<TInstance> context, IBehavior<TInstance> next)
    {
        return next.Execute(context);
    }

    Task IActivity<TInstance>.Execute<T>(IBehaviorContext<TInstance, T> context, IBehavior<TInstance, T> next)
    {
        return next.Execute(context);
    }

    async Task IActivity<TInstance>.Faulted<T>(IBehaviorExceptionContext<TInstance, T> context, IBehavior<TInstance> next)
    {
        var exceptionContext = context as IBehaviorExceptionContext<TInstance, TException>;
        if (exceptionContext != null)
        {
            await _behavior.Faulted(exceptionContext).ConfigureAwait(false);

            // if the compensate returns, we should go forward normally
            await next.Execute(context).ConfigureAwait(false);
        }
        else
        {
            await next.Faulted(context).ConfigureAwait(false);
        }
    }

    async Task IActivity<TInstance>.Faulted<TData, T>(IBehaviorExceptionContext<TInstance, TData, T> context,
        IBehavior<TInstance, TData> next)
    {
        var exceptionContext = context as IBehaviorExceptionContext<TInstance, TData, TException>;
        if (exceptionContext != null)
        {
            await _behavior.Faulted(exceptionContext).ConfigureAwait(false);

            // if the compensate returns, we should go forward normally
            await next.Execute(context).ConfigureAwait(false);
        }
        else
        {
            await next.Faulted(context).ConfigureAwait(false);
        }
    }
}