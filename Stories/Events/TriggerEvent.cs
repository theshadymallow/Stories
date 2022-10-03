using GreenPipes;
using Stories.Abstractions;

namespace Stories.Events;

public class TriggerEvent : IEvent
{
    private readonly string _name;

    public TriggerEvent(string name)
    {
        _name = name;
    }
    
    public void Probe(ProbeContext context)
    {
        throw new NotImplementedException();
    }

    public void Accept(IStateMachineVisitor visitor)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(IEvent? other)
    {
        throw new NotImplementedException();
    }

    public string Name { get; }
    
    public bool Equals(TriggerEvent other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Equals(other._name, _name);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != typeof(TriggerEvent))
            return false;
        return Equals((TriggerEvent)obj);
    }

    public override int GetHashCode()
    {
        return _name?.GetHashCode() ?? 0;
    }

    public override string ToString()
    {
        return $"{_name} (Event)";
    }
}