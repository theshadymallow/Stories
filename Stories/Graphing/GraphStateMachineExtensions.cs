using Stories.Abstractions;

namespace Stories.Graphing;

public static class GraphStateMachineExtensions
{
    public static StateMachineGraph GetGraph<TInstance>(this IStateMachine<TInstance> machine)
        where TInstance : class
    {
        var inspector = new GraphStateMachineVisitor<TInstance>();

        machine.Accept(inspector);

        return inspector.Graph;
    }
}