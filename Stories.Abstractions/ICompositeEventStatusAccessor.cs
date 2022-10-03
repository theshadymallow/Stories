using GreenPipes;

namespace Stories.Abstractions;

public interface ICompositeEventStatusAccessor<in TInstance> : IProbeSite
{
    CompositeEventStatus Get(TInstance instance);

    void Set(TInstance instance, CompositeEventStatus status);
}