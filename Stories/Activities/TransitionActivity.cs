using GreenPipes;
using Stories.Abstractions;

namespace Stories.Activities;

public class TransitionActivity<TInstance> :
    IActivity<TInstance>
    where TInstance : class
{
    private readonly IStateAccessor<TInstance> _currentStateAccessor;
    private readonly IState<TInstance> _toState;

    public TransitionActivity(IState<TInstance> toState, IStateAccessor<TInstance> currentStateAccessor)
    {
        _toState = toState;
        _currentStateAccessor = currentStateAccessor;
    }

    public IState ToState => _toState;

    void IVisitable.Accept(IStateMachineVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Probe(ProbeContext context)
    {
        var scope = context.CreateScope("transition");
        scope.Add("toState", _toState.Name);
    }

    async Task IActivity<TInstance>.Execute(IBehaviorContext<TInstance> context, IBehavior<TInstance> next)
    {
        await Transition(context).ConfigureAwait(false);

        await next.Execute(context).ConfigureAwait(false);
    }

    async Task IActivity<TInstance>.Execute<TData>(IBehaviorContext<TInstance, TData> context, IBehavior<TInstance, TData> next)
    {
        await Transition(context).ConfigureAwait(false);

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

    async Task Transition(IBehaviorContext<TInstance> context)
    {
        var currentState = await _currentStateAccessor.Get(context).ConfigureAwait(false);
        if (_toState.Equals(currentState))
            return; // Homey don't play re-entry, at least not yet.

        if (currentState != null && !currentState.HasState(_toState))
            await RaiseCurrentStateLeaveEvents(context, currentState).ConfigureAwait(false);

        await RaiseBeforeEnterEvents(context, currentState, _toState).ConfigureAwait(false);

        await _currentStateAccessor.Set(context, _toState).ConfigureAwait(false);

        if (currentState != null)
            await RaiseAfterLeaveEvents(context, currentState, _toState).ConfigureAwait(false);

        if (currentState == null || !_toState.HasState(currentState))
        {
            var superState = _toState.SuperState;
            while (superState != null && (currentState == null || !superState.HasState(currentState)))
            {
                var superStateEnterContext = context.GetProxy(superState.Enter);
                await superState.Raise(superStateEnterContext).ConfigureAwait(false);

                superState = superState.SuperState;
            }

            var enterContext = context.GetProxy(_toState.Enter);
            await _toState.Raise(enterContext).ConfigureAwait(false);
        }
    }

    async Task RaiseBeforeEnterEvents(IBehaviorContext<TInstance> context, IState<TInstance> currentState, IState<TInstance> toState)
    {
        var superState = toState.SuperState;
        if (superState != null && (currentState == null || !superState.HasState(currentState)))
            await RaiseBeforeEnterEvents(context, currentState, superState).ConfigureAwait(false);

        if (currentState != null && toState.HasState(currentState))
            return;

        var beforeContext = context.GetProxy(toState.BeforeEnter, toState);
        await toState.Raise(beforeContext).ConfigureAwait(false);
    }

    async Task RaiseAfterLeaveEvents(IBehaviorContext<TInstance> context, IState<TInstance> fromState, IState<TInstance> toState)
    {
        if (fromState.HasState(toState))
            return;

        var afterContext = context.GetProxy(fromState.AfterLeave, fromState);
        await fromState.Raise(afterContext).ConfigureAwait(false);

        var superState = fromState.SuperState;
        if (superState != null)
            await RaiseAfterLeaveEvents(context, superState, toState).ConfigureAwait(false);
    }

    async Task RaiseCurrentStateLeaveEvents(IBehaviorContext<TInstance> context, IState<TInstance> fromState)
    {
        var leaveContext = context.GetProxy(fromState.Leave);
        await fromState.Raise(leaveContext).ConfigureAwait(false);

        var superState = fromState.SuperState;
        while (superState != null && !superState.HasState(_toState))
        {
            var superStateLeaveContext = context.GetProxy(superState.Leave);
            await superState.Raise(superStateLeaveContext).ConfigureAwait(false);

            superState = superState.SuperState;
        }
    }
}