using System.Linq.Expressions;
using GreenPipes;

namespace Stories.Abstractions;

public interface IStateAccessor<TInstance> :
    IProbeSite
{
    Task<IState<TInstance>> Get(IInstanceContext<TInstance> context);

    Task Set(IInstanceContext<TInstance> context, IState<TInstance> state);

    /// <summary>
    /// Converts a state expression to the instance current state property type.
    /// </summary>
    /// <param name="states"></param>
    /// <returns></returns>
    Expression<Func<TInstance, bool>> GetStateExpression(params IState[] states);
}