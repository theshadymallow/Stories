using System.Reflection;
using Stories.Abstractions;
using Stories.Activities;
using Stories.Events;

namespace Stories.Graphing;

public class GraphStateMachineVisitor<TInstance> :
        IStateMachineVisitor
        where TInstance : class
{
    private readonly HashSet<Edge> _edges;
    private readonly Dictionary<IEvent, Vertex> _events;
    private readonly Dictionary<IState, Vertex> _states;
    
    private Vertex _currentEvent;
    private Vertex _currentState;

    public GraphStateMachineVisitor()
    {
        _edges = new HashSet<Edge>();
        _states = new Dictionary<IState, Vertex>();
        _events = new Dictionary<IEvent, Vertex>();
    }

    public StateMachineGraph Graph
    {
        get
        {
            var events = _events.Values
                .Where(e => _edges.Any(edge => edge.From.Equals(e)));

            var states = _states.Values
                .Where(s => _edges.Any(edge => edge.From.Equals(s) || edge.To.Equals(s)));

            var vertices = new HashSet<Vertex>(states.Union(events));

            var edges = _edges
                .Where(e => vertices.Contains(e.From) && vertices.Contains(e.To));

            return new StateMachineGraph(vertices, edges);
        }
    }

    public void Visit(IState state, Action<IState> next)
    {
        _currentState = GetStateVertex(state);

        next(state);
    }

    public void Visit(IEvent @event, Action<IEvent> next)
    {
        _currentEvent = GetEventVertex(@event);

        _edges.Add(new Edge(_currentState, _currentEvent, _currentEvent.Title));

        next(@event);
    }

    public void Visit<TData>(IEvent<TData> @event, Action<IEvent<TData>> next)
    {
        _currentEvent = GetEventVertex(@event);

        _edges.Add(new Edge(_currentState, _currentEvent, _currentEvent.Title));

        next(@event);
    }

    public void Visit(IActivity activity)
    {
        Visit(activity, x => { });
    }

    public void Visit<T>(IBehavior<T> behavior)
    {
        Visit(behavior, x => { });
    }

    public void Visit<T>(IBehavior<T> behavior, Action<IBehavior<T>> next)
    {
        next(behavior);
    }

    public void Visit<T, TData>(IBehavior<T, TData> behavior)
    {
        Visit(behavior, x => { });
    }

    public void Visit<T, TData>(IBehavior<T, TData> behavior, Action<IBehavior<T, TData>> next)
    {
        next(behavior);
    }

    public void Visit(IActivity activity, Action<IActivity> next)
    {
        if (activity is TransitionActivity<TInstance> transitionActivity)
        {
            InspectTransitionActivity(transitionActivity);
            next(activity);
            return;
        }

        if (activity is CompositeEventActivity<TInstance> compositeActivity)
        {
            InspectCompositeEventActivity(compositeActivity);
            next(activity);
            return;
        }

        var activityType = activity.GetType();
        var compensateType = activityType.GetTypeInfo().IsGenericType
                             && activityType.GetGenericTypeDefinition() == typeof(CatchFaultActivity<,>)
            ? activityType.GetGenericArguments().Skip(1).First()
            : null;

        if (compensateType != null)
        {
            var previousEvent = _currentEvent;

            var eventType = typeof(DataEvent<>).MakeGenericType(compensateType);
            var evt = (IEvent)Activator.CreateInstance(eventType, compensateType.Name);
            _currentEvent = GetEventVertex(evt);

            _edges.Add(new Edge(previousEvent, _currentEvent, _currentEvent.Title));

            next(activity);

            _currentEvent = previousEvent;
            return;
        }

        next(activity);
    }

    void InspectCompositeEventActivity(CompositeEventActivity<TInstance> compositeActivity)
    {
        var previousEvent = _currentEvent;

        _currentEvent = GetEventVertex(compositeActivity.Event);

        _edges.Add(new Edge(previousEvent, _currentEvent, _currentEvent.Title));
    }

    //TODO: I don't think I want to inspect the exception activity. It would slow down the parent thread. MAKE SURE YOU HAVE THIS REVIEWED MARSHALL
    
//        void InspectExceptionActivity(ExceptionActivity<TInstance> exceptionActivity, Action<Activity> next)
//        {
//            Vertex previousEvent = _currentEvent;
//
//            _currentEvent = GetEventVertex(exceptionActivity.Event);
//
//            _edges.Add(new Edge(previousEvent, _currentEvent, _currentEvent.Title));
//
//            next(exceptionActivity);
//
//            _currentEvent = previousEvent;
//        }

//        void InspectTryActivity(TryActivity<TInstance> exceptionActivity, Action<Activity> next)
//        {
//            Vertex previousEvent = _currentEvent;
//
//            next(exceptionActivity);
//
//            _currentEvent = previousEvent;
//        }

    void InspectTransitionActivity(TransitionActivity<TInstance> transitionActivity)
    {
        var targetState = GetStateVertex(transitionActivity.ToState);

        _edges.Add(new Edge(_currentEvent, targetState, _currentEvent.Title));
    }

    Vertex GetStateVertex(IState state)
    {
        if (_states.TryGetValue(state, out var vertex))
            return vertex;

        vertex = CreateStateVertex(state);
        _states.Add(state, vertex);

        return vertex;
    }

    Vertex GetEventVertex(IEvent state)
    {
        if (_events.TryGetValue(state, out var vertex))
            return vertex;

        vertex = CreateEventVertex(state);
        _events.Add(state, vertex);

        return vertex;
    }

    static Vertex CreateStateVertex(IState state)
    {
        return new Vertex(typeof(IState), typeof(IState), state.Name);
    }

    static Vertex CreateEventVertex(IEvent @event)
    {
        var targetType = @event
            .GetType()
            .GetInterfaces()
            .Where(x => x.GetTypeInfo().IsGenericType)
            .Where(x => x.GetGenericTypeDefinition() == typeof(IEvent<>))
            .Select(x => x.GetGenericArguments()[0])
            .DefaultIfEmpty(typeof(IEvent))
            .Single();

        return new Vertex(typeof(IEvent), targetType, @event.Name);
    }

    static Vertex CreateEventVertex(Type exceptionType)
    {
        return new Vertex(typeof(IEvent), exceptionType, exceptionType.Name);
    }
}