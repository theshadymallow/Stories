namespace Stories.Abstractions;

public interface IActivity :
        IVisitable
    {
    }


    /// <summary>
    /// An activity is part of a behavior that is executed in order
    /// </summary>
    /// <typeparam name="TInstance"></typeparam>
    public interface IActivity<TInstance> :
        IActivity
    {
        /// <summary>
        /// Execute the activity with the given behavior context
        /// </summary>
        /// <param name="context">The behavior context</param>
        /// <param name="next">The behavior that follows this activity</param>
        /// <returns>An awaitable task</returns>
        Task Execute(IBehaviorContext<TInstance> context, IBehavior<TInstance> next);

        /// <summary>
        /// Execute the activity with the given behavior context
        /// </summary>
        /// <param name="context">The behavior context</param>
        /// <param name="next">The behavior that follows this activity</param>
        /// <returns>An awaitable task</returns>
        Task Execute<T>(IBehaviorContext<TInstance, T> context, IBehavior<TInstance, T> next);

        /// <summary>
        /// The exception path through the behavior allows activities to catch and handle exceptions
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task Faulted<TException>(IBehaviorExceptionContext<TInstance, TException> context, IBehavior<TInstance> next)
            where TException : Exception;

        /// <summary>
        /// The exception path through the behavior allows activities to catch and handle exceptions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task Faulted<T, TException>(IBehaviorExceptionContext<TInstance, T, TException> context, IBehavior<TInstance, T> next)
            where TException : Exception;
    }


    public interface IActivity<TInstance, TData> :
        IActivity
    {
        /// <summary>
        /// Execute the activity with the given behavior context
        /// </summary>
        /// <param name="context">The behavior context</param>
        /// <param name="next">The behavior that follows this activity</param>
        /// <returns>An awaitable task</returns>
        Task Execute(IBehaviorContext<TInstance, TData> context, IBehavior<TInstance, TData> next);

        /// <summary>
        /// The exception path through the behavior allows activities to catch and handle exceptions
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task Faulted<TException>(IBehaviorExceptionContext<TInstance, TData, TException> context, IBehavior<TInstance, TData> next)
            where TException : Exception;
    }