namespace Stories.Abstractions;

public interface IEventContext<out TInstance> :
    IInstanceContext<TInstance>
{
    IEvent Event { get; }

    /// <summary>
    /// Raise an event on the current instance, pushing the current event on the stack
    /// </summary>
    /// <param name="event">The event to raise</param>
    /// <returns>An awaitable Task</returns>
    Task Raise(IEvent @event);

    /// <summary>
    /// Raise an event on the current instance, pushing the current event on the stack
    /// </summary>
    /// <param name="event">The event to raise</param>
    /// <param name="data">THe event data</param>
    /// <returns>An awaitable Task</returns>
    Task Raise<TData>(IEvent<TData> @event, TData data);
}


/// <summary>
/// Encapsulates an event that was raised which includes data
/// </summary>
/// <typeparam name="TInstance">The state instance the event is targeting</typeparam>
/// <typeparam name="TData">The event data type</typeparam>
public interface IEventContext<out TInstance, out TData> :
    IEventContext<TInstance>
{
    new IEvent<TData> Event { get; }

    /// <summary>
    /// The data from the event
    /// </summary>
    TData Data { get; }
}