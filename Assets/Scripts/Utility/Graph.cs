using System.Collections.Generic;

/// <summary>
/// A very low-level, generic Graph data structure. This class is intended to
/// be extended and built upon, not used on its own for any real heavy lifting.
/// </summary>
public class Graph<T> where T : System.IEquatable<T> {
    private readonly List<Vertex<T>> _adjacencyList;
    /// <summary>Exposes the Adjacency List for algorithmic convenience.</summary>
    public IEnumerable<Vertex<T>> AdjacencyList { get { return _adjacencyList; } }

    public Graph() {
        _adjacencyList = new List<Vertex<T>>();
    }

    /// <summary>Creates the Graph with the specified number of vertices.</summary>
    /// <param>The number of Vertices to allocate for the Graph.</param>
    public Graph(int initialSize) {
        if (initialSize > 0) {
            _adjacencyList = new List<Vertex<T>>(initialSize);
        }
    }

    /// <summary>Adds a Vertex into the Graph.</summary>
    /// <param>Generic type to add to the Graph.</summary>
    public void AddVertex(T toAdd) {
        _adjacencyList.Add(new Vertex<T>(toAdd));
    }

    /// <summary>Adds an edge between the specified vertices.</summary>
    /// <param name="source">The source vertex.</param>
    /// <param name="destination">The destination vertex.</param>
    /// <remarks>
    /// The edge is inserted from Source -> Destination. Remember, this is an
    /// unweighted Graph, and this method only inserts an edge in one direction.
    /// </remarks>
    public bool AddEdge(T source, T destination) {
        if (source == null || destination == null) {
            return false;
        }
        foreach (var vertex in _adjacencyList) {
            if (vertex.Equals(source)) {
                //vertex.
            }
        }
        return true;
    }

    /// <summary>Represents a Vertex node in a Graph.</summary>
    public class Vertex<V> {
        /// <summary>The data held by the Vertex.</summary>
        public V Data { get; private set; }
        /// <summary>The list of adjacent Vertices.</summary>
        private readonly List<Edge> _edgeList = new List<Edge>();

        /// <summary>Constructs the Vertex with the initial value.</summary>
        public Vertex(V initialValue) {
            Data = initialValue;    
        }

        public void AddEdge(Vertex<T> destination) {
            _edgeList.Add(new Edge(destination));
        }
    }

    /// <summary>Represents an Edge between two Vertices.</summary>
    public class Edge {
        /// <summary>A reference to the Vertex that this edge leads to.</summary>
         public Vertex<T> AdjacentTo { get; set; }

         public Edge(Vertex<T> adjacentTo) {
             AdjacentTo = adjacentTo;
         }
    }
}
