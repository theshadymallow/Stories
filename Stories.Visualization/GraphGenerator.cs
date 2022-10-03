namespace Stories.Visualization;

using Stories.Graphing;
using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

public class GraphGenerator
{
    private readonly AdjacencyGraph<Vertex, Edge<Vertex>> _graph;
    
    public GraphGenerator(StateMachineGraph data)
    {
        _graph = CreateAdjacencyGraph(data);
    }

    static AdjacencyGraph<Vertex, Edge<Vertex>> CreateAdjacencyGraph(StateMachineGraph data)
    {
        var graph = new AdjacencyGraph<Vertex, Edge<Vertex>>();
        graph.AddVertexRange(data.Vertices);
        graph.AddEdgeRange(data.Edges.Select(x => new Edge<Vertex>(x.From, x.To)));
        return graph;
    }

}