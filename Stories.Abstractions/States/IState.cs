namespace Stories.Abstractions;

public interface IState :
        IVisitable,
        IComparable<IState>
    {
        string Name { get; }

        /// <summary>
        /// Raised when the state is entered
        /// </summary>
        IEvent Enter { get; }

        /// <summary>
        /// Raised when the state is about to be left
        /// </summary>
        IEvent Leave { get; }

        /// <summary>
        /// Raised just before the state is about to change to a new state
        /// </summary>
        IEvent<IState> BeforeEnter { get; }

        /// <summary>
        /// Raised just after the state has been left and a new state is selected
        /// </summary>
        IEvent<IState> AfterLeave { get; }
    }


    /// <summary>
    /// A state within a state machine that can be targeted with events
    /// </summary>
    /// <typeparam name="TInstance">The instance type to which the state applies</typeparam>
    public interface IState<TInstance> :
        IState
    {
        IEnumerable<IEvent> Events { get; }

        /// <summary>
        /// Returns the superState of the state, if there is one
        /// </summary>
        IState<TInstance> SuperState { get; }

        Task Raise(IEventContext<TInstance> context);

        /// <summary>
        /// Raise an event to the state, passing the instance
        /// </summary>
        /// <typeparam name="T">The event data type</typeparam>
        /// <param name="context">The event context</param>
        /// <returns></returns>
        Task Raise<T>(IEventContext<TInstance, T> context);

        /// <summary>
        /// Bind an activity to an event
        /// </summary>
        /// <param name="event"></param>
        /// <param name="activity"></param>
        void Bind(IEvent @event, IActivity<TInstance> activity);

        /// <summary>
        /// Ignore the specified event in this state. Prevents an exception from being thrown if
        /// the event is raised during this state.
        /// </summary>
        /// <param name="event"></param>
        void Ignore(IEvent @event);

        /// <summary>
        /// Ignore the specified event in this state if the filter condition passed. Prevents exceptions
        /// from being thrown if the event is raised during this state.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <param name="filter"></param>
        void Ignore<T>(IEvent<T> @event, StateMachineEventFilter<TInstance, T> filter);

        /// <summary>
        /// Adds a substate to the state
        /// </summary>
        /// <param name="subState"></param>
        void AddSubstate(IState<TInstance> subState);

        /// <summary>
        /// True if the specified state is included in the state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        bool HasState(IState<TInstance> state);

        /// <summary>
        /// True if the specified state is a substate of the current state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        bool IsStateOf(IState<TInstance> state);
    }