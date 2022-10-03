
namespace Stories.Visualization;

using System;

using QuikGraph;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

using Stories.Abstractions;
using Graphing;

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

    static void VertexStyler(object sender, FormatVertexEventArgs<Vertex> args)
    {
        args.VertexFormat.Label = args.Vertex.Title;
        
        //Format the shape/font color/label of a graph's vertex if that vertex is an event
        if (args.Vertex.VertexType == typeof(IEvent))
        {
            args.VertexFormat.FontColor = GraphvizColor.Black;
            args.VertexFormat.Shape = GraphvizVertexShape.Rectangle;
            
            //If the vertex is an exception event (It will implement IEvent and Exception), style it accordingly
            if (args.Vertex.TargetType != typeof(IEvent) && args.Vertex.TargetType != typeof(Exception))
                args.VertexFormat.Label += "<" + args.Vertex.TargetType.Name + ">";
        }

        //If the vertex isn't an event, its the start or stop of the state machine. Format the matrix accordingly
        switch (args.Vertex.Title)
        {
            case "Initial":
                args.VertexFormat.FillColor = GraphvizColor.White;
                break;
            case "Final":
                args.VertexFormat.FillColor = GraphvizColor.White;
                break;
            default:
                args.VertexFormat.FillColor = GraphvizColor.White;
                args.VertexFormat.FontColor = GraphvizColor.Black;
                break;
        }
        
        args.VertexFormat.Shape = GraphvizVertexShape.Ellipse;
    }

    /// <summary>
    /// This is the method
    /// </summary>
    /// <returns></returns>
    public string CreateDotFile()
    {
        var algo = new GraphvizAlgorithm<Vertex, Edge<Vertex>>(_graph);
        //Call the formatter
        algo.FormatVertex += VertexStyler;
        return algo.Generate();
    }

}