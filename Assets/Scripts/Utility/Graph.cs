using System.Collections.Generic;
using System.Linq;

/// <summary>
/// A very low-level, generic Graph data structure. It provides 
/// convenient access to the Vertices by exposing an enumerable
/// Adjacency List.
/// </summary>
/// <remarks>
/// This class is intended to be extended and built upon, not used 
/// on its own for any real heavy lifting. Due to its generic nature,
/// it is assumed that whoever is extending this class will provide
/// logic for distinguishing Vertices from one another for the purposes
/// of methods like AddEdge, etc. This class does not presume to perform
/// such operations nor does it assume the data supports it. This is the
/// responsibility of the client code. That is exactly why some of the 
/// methods take in generic Vertex as parameters.
/// </remarks>
public class Graph<T> {
    private readonly List<Vertex> _adjacencyList;
    /// <summary>Exposes the Adjacency List for algorithmic convenience.</summary>
    public IEnumerable<Vertex> AdjacencyList { get { return _adjacencyList; } }
    /// <summary>The number of Vertices in the Graph.</summary>
    public int Count { get { return _adjacencyList.Count ; } }

    /// <summary>Creates the Graph with the specified number of vertices.</summary>
    /// <param name="initialSize">
    /// The number of Vertices to allocate for the Graph.
    /// </param>
    public Graph(int initialSize = 12) {
        if (initialSize > 0) {
            _adjacencyList = new List<Vertex>(initialSize);
        }
    }

    /// <summary>Adds a Vertex into the Graph.</summary>
    /// <param>Generic type to add to the Graph.</summary>
    public void AddVertex(T toAdd) {
        _adjacencyList.Add(new Vertex(toAdd));
    }

    /// <summary>Removes the specified Vertex from the Graph.</summary>
    /// <param name="toRemove">A reference to the Vertex to be removed.</param>
    public bool RemoveVertex(Vertex toRemove) {
        foreach (var vertex in _adjacencyList) {
            vertex.RemoveEdge(toRemove);
        }

        return _adjacencyList.Remove(toRemove);
    }

    /// <summary>Adds an edge between the specified vertices.</summary>
    /// <param name="source">The source vertex.</param>
    /// <param name="destination">The destination vertex.</param>
    /// <remarks>The edge is inserted from Source -> Destination.</remarks>
    /// <returns>
    /// True if an edge was established, False if one or more arguments
    /// could not be resolved to valid vertices.
    /// </returns>
    public bool AddEdge(Vertex source, Vertex destination) {
        if (source == null || destination == null) {
            return false;
        }

        source.AddEdge(destination);

        return true;
    }

    /// <summary>Removes the edge from one vertex to another.</summary>
    /// <param name="removeFrom">The Vertex to remove the edge from.</param>
    /// <param name="toRemove">The Vertex that will no longer be adjacent to removeFrom.</param>
    /// <remarks>removeFrom's edge will be removed from toRemove.</remarks>
    public bool RemoveEdge(Vertex removeFrom, Vertex toRemove) {
        return removeFrom.RemoveEdge(toRemove);
    }

    /// <summary>
    /// Provides an IEnumerable to iterate over all adjacent 
    /// vertices to the specified vertex.
    /// </summary>
    /// <param name="vertex">The Vertex whose neighbors to iterate over.</param>
    public IEnumerable<Vertex> Neighbors(Vertex vertex) {
        return vertex.Neighbors();
    }

    /// <summary>
    /// Clears/removes all vertices from the Graph.
    /// </summary>
    public void Clear() {
        _adjacencyList.Clear();
    }

    /// <summary>Represents a Vertex node in a Graph.</summary>
    public class Vertex {
        /// <summary>The data held by the Vertex.</summary>
        public T Data { get; private set; }
        /// <summary>The number of adjacent vertices to this one.</summary>
        public int CountAdjacent { get { return _edgeList.Count; } }
        /// <summary>The list of adjacent Vertices.</summary>
        private readonly List<Edge> _edgeList = new List<Edge>();

        /// <summary>Constructs the Vertex with the initial value.</summary>
        public Vertex(T initialValue) {
            Data = initialValue;    
        }

        /// <summary>Adds an Edge from this Vertex to the provided Vertex.</summary>
        /// <param name="destination">The Vertex being made adjacent to this one.</param>
        public void AddEdge(Vertex destination) {
            _edgeList.Add(new Edge(this, destination));
        }

        /// <summary>Remove all edges from this Vertex to the specified Vertex.</summary>
        /// <param name="detachFrom">The Vertex to sever ties from.</param>
        /// <returns>Returns true if items were removed, false if 0 items were removed.</returns>
        public bool RemoveEdge(Vertex detachFrom) {
            if (detachFrom == null) {
                return false;
            }

            // RemoveAll returns # removed, if it's not zero, success.
            return (_edgeList.RemoveAll(e => e.To == detachFrom) != 0);
        }

        /// <summary>Provides an enumerable collection of Vertices adjacent to this one.</summary>
        /// <returns>An IEnumerable of adjacent Vertices.</returns>
        public IEnumerable<Vertex> Neighbors() {
            return _edgeList.Select(edge => edge.To).ToList();
        }
    }

    /// <summary>Represents an Edge between two Vertices.</summary>
    public class Edge {
        /// <summary>A reference to the Vertex that this edge leads to.</summary>
         public Vertex To { get; set; }
         public Vertex From { get; set; }

         public Edge(Vertex adjacentFrom, Vertex adjacentTo) {
             To = adjacentTo;
             From = adjacentFrom;
         }
    }
}
