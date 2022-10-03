using GreenPipes;

namespace Stories.Abstractions;

public interface IVisitable : IProbeSite
{
    /// <summary>
    /// A visitable object can accept the visitor and pass control to internal elements (Expose previously private accessors)
    /// </summary>
    /// <param name="visitor"></param>
    void Accept(IStateMachineVisitor visitor);
}