namespace Stories.Abstractions;

public interface IEventObserver<in TInstance>
    {
        /// <summary>
        /// Called before the event context is delivered to the activities
        /// </summary>
        /// <param name="context">The event context</param>
        /// <returns></returns>
        Task PreExecute(IEventContext<TInstance> context);

        /// <summary>
        /// Called before the event context is delivered to the activities
        /// </summary>
        /// <typeparam name="T">The event data type</typeparam>
        /// <param name="context">The event context</param>
        /// <returns></returns>
        Task PreExecute<T>(IEventContext<TInstance, T> context);

        /// <summary>
        /// Called when the event has been processed by the activities
        /// </summary>
        /// <param name="context">The event context</param>
        /// <returns></returns>
        Task PostExecute(IEventContext<TInstance> context);

        /// <summary>
        /// Called when the event has been processed by the activities
        /// </summary>
        /// <typeparam name="T">The event data type</typeparam>
        /// <param name="context">The event context</param>
        /// <returns></returns>
        Task PostExecute<T>(IEventContext<TInstance, T> context);

        /// <summary>
        /// Called when the activity execution faults and is not handled by the activities
        /// </summary>
        /// <param name="context">The event context</param>
        /// <param name="exception">The exception that was thrown</param>
        /// <returns></returns>
        Task ExecuteFault(IEventContext<TInstance> context, Exception exception);

        /// <summary>
        /// Called when the activity execution faults and is not handled by the activities
        /// </summary>
        /// <typeparam name="T">The message type</typeparam>
        /// <param name="context">The event context</param>
        /// <param name="exception">The exception that was thrown</param>
        /// <returns></returns>
        Task ExecuteFault<T>(IEventContext<TInstance, T> context, Exception exception);
    }