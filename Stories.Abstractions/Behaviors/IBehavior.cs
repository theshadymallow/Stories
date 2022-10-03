namespace Stories.Abstractions;

/// <summary>
/// A behavior is a chain of activities invoked by a state
/// </summary>
/// <typeparam name="TInstance">The state type</typeparam>
public interface IBehavior<in TInstance> :
    IVisitable
{
    /// <summary>
    /// Execute the activity with the given behavior context
    /// </summary>
    /// <param name="context">The behavior context</param>
    /// <returns>An awaitable task</returns>
    Task Execute(IBehaviorContext<TInstance> context);

    /// <summary>
    /// Execute the activity with the given behavior context
    /// </summary>
    /// <param name="context">The behavior context</param>
    /// <returns>An awaitable task</returns>
    Task Execute<T>(IBehaviorContext<TInstance, T> context);

    /// <summary>
    /// The exception path through the behavior allows activities to catch and handle exceptions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    Task Faulted<T, TException>(IBehaviorExceptionContext<TInstance, T, TException> context)
        where TException : Exception;

    /// <summary>
    /// The exception path through the behavior allows activities to catch and handle exceptions
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    Task Faulted<TException>(IBehaviorExceptionContext<TInstance, TException> context)
        where TException : Exception;
}


/// <summary>
/// A behavior is a chain of activities invoked by a state
/// </summary>
/// <typeparam name="TInstance">The state type</typeparam>
/// <typeparam name="TData">The data type of the behavior</typeparam>
public interface IBehavior<in TInstance, in TData> :
    IVisitable
{
    /// <summary>
    /// Execute the activity with the given behavior context
    /// </summary>
    /// <param name="context">The behavior context</param>
    /// <returns>An awaitable task</returns>
    Task Execute(IBehaviorContext<TInstance, TData> context);

    /// <summary>
    /// The exception path through the behavior allows activities to catch and handle exceptions
    /// </summary>
    /// <typeparam name="TException"></typeparam>
    /// <param name="context"></param>
    /// <returns></returns>
    Task Faulted<TException>(IBehaviorExceptionContext<TInstance, TData, TException> context)
        where TException : Exception;
}