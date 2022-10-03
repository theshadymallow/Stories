using GreenPipes;
using GreenPipes.Util;
using Stories.Abstractions;

namespace Stories.Activities;

public class CompositeEventActivity<TInstance> :
    IActivity<TInstance>
    where TInstance : class
{
    private readonly ICompositeEventStatusAccessor<TInstance> _accessor;
    private readonly CompositeEventStatus _complete;
    private readonly IEvent _event;
    private readonly int _flag;

    public CompositeEventActivity(ICompositeEventStatusAccessor<TInstance> accessor, int flag, CompositeEventStatus complete,
        IEvent @event)
    {
        _accessor = accessor;
        _flag = flag;
        _complete = complete;
        _event = @event;
    }

    public IEvent Event => _event;

    void IVisitable.Accept(IStateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Probe(ProbeContext context)
    {
        var scope = context.CreateScope("compositeEvent");
        _accessor.Probe(scope);
        scope.Add("event", _event.Name);
        scope.Add("flag", _flag.ToString("X8"));
    }

    async Task IActivity<TInstance>.Execute(IBehaviorContext<TInstance> context, IBehavior<TInstance> next)
    {
        await Execute(context).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    async Task IActivity<TInstance>.Execute<TData>(IBehaviorContext<TInstance, TData> context, IBehavior<TInstance, TData> next)
    {
        await Execute(context).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    Task IActivity<TInstance>.Faulted<TException>(IBehaviorExceptionContext<TInstance, TException> context, IBehavior<TInstance> next)
    {
        return next.Faulted(context);
    }
    
    Task IActivity<TInstance>.Faulted<T, TException>(IBehaviorExceptionContext<TInstance, T, TException> context,
        IBehavior<TInstance, T> next)
    {
        return next.Faulted(context);
    }

    Task Execute(IBehaviorContext<TInstance> context)
    {
        var value = _accessor.Get(context.Instance);
        value.Set(_flag);

        _accessor.Set(context.Instance, value);

        if (!value.Equals(_complete))
            return TaskUtil.Completed;

        return RaiseCompositeEvent(context);
    }

    Task RaiseCompositeEvent(IBehaviorContext<TInstance> context)
    {
        return context.Raise(_event);
    }
}