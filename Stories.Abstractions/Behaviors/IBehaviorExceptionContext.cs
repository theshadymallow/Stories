namespace Stories.Abstractions;

/// <summary>
/// An exceptional behavior context
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TData"></typeparam>
/// <typeparam name="TException"></typeparam>
public interface IBehaviorExceptionContext<out TInstance, out TData, out TException> :
    IBehaviorContext<TInstance, TData>,
    IBehaviorExceptionContext<TInstance, TException>
    where TException : Exception
{
    /// <summary>
    /// Return a proxy of the current behavior context with the specified event and data
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    /// <param name="event">The event for the new context</param>
    /// <param name="data">The data for the event</param>
    /// <returns></returns>
    new IBehaviorExceptionContext<TInstance, T, TException> GetProxy<T>(IEvent<T> @event, T data);
}


/// <summary>
/// An exceptional behavior context
/// </summary>
/// <typeparam name="TInstance"></typeparam>
/// <typeparam name="TException"></typeparam>
public interface IBehaviorExceptionContext<out TInstance, out TException> :
    IBehaviorContext<TInstance>
    where TException : Exception
{
    TException Exception { get; }

    /// <summary>
    /// Return a proxy of the current behavior context with the specified event and data
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    /// <param name="event">The event for the new context</param>
    /// <param name="data">The data for the event</param>
    /// <returns></returns>
    new IBehaviorExceptionContext<TInstance, T, TException> GetProxy<T>(IEvent<T> @event, T data);
}