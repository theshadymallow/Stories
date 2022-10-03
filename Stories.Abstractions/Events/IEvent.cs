namespace Stories.Abstractions;

public interface IEvent : IVisitable , IComparable<IEvent>
{
    string Name { get; }
}

public interface IEvent<out TData> :
    IEvent
{
}