using Stories.Abstractions;

namespace Stories.Events;

public class DataEvent<TData> : TriggerEvent, IEvent<TData>, IEquatable<DataEvent<TData>>
{
    public bool Equals(DataEvent<TData>? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DataEvent<TData>) obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public DataEvent(string name) : base(name)
    {
    }
}