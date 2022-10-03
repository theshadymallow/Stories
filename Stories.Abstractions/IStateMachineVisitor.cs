namespace Stories.Abstractions;

public interface IStateMachineVisitor
{
    void Visit(IState state, Action<IState> next);

    void Visit(IEvent @event, Action<IEvent> next);

    void Visit<TData>(IEvent<TData> @event, Action<IEvent<TData>> next);

    void Visit(IActivity activity);

    void Visit<T>(IBehavior<T> behavior);

    void Visit<T>(IBehavior<T> behavior, Action<IBehavior<T>> next);

    void Visit<T, TData>(IBehavior<T, TData> behavior);
    void Visit<T, TData>(IBehavior<T, TData> behavior, Action<IBehavior<T, TData>> next);

    void Visit(IActivity activity, Action<IActivity> next);
}