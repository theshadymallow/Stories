using GreenPipes;

namespace Stories.Abstractions;

public interface IInstanceContext<out TInstance> :
    PipeContext
{
    /// <summary>
    /// The state instance which is targeted by the event
    /// </summary>
    TInstance Instance { get; }
}