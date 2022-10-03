namespace Stories.Graphing;

[Serializable]
public class StateMachineGraph
{
    private readonly Edge[] _edges;
    private readonly Vertex[] _vertices;

    public StateMachineGraph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
    {
        _vertices = vertices.ToArray();
        _edges = edges.ToArray();
    }

    public IEnumerable<Vertex> Vertices => _vertices;

    public IEnumerable<Edge> Edges => _edges;
}