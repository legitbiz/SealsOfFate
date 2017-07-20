using System.Collections.Generic;

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
    private readonly List<Vertex<T>> _adjacencyList;
    /// <summary>Exposes the Adjacency List for algorithmic convenience.</summary>
    public IEnumerable<Vertex<T>> AdjacencyList { get { return _adjacencyList; } }
    /// <summary>The number of Vertices in the Graph.</summary>
    public int Count { get { return _adjacencyList.Count ; } }

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

    /// <summary>Removes the specified Vertex from the Graph.</summary>
    /// <param name="toRemove">A reference to the Vertex to be removed.</param>
    public bool RemoveVertex(Vertex<T> toRemove) {
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
    public bool AddEdge(Vertex<T> source, Vertex<T> destination) {
        if (source == null || destination == null) {
            return false;
        }

        source.AddEdge(destination);

        return true;
    }

    public IEnumerable<Vertex<T>> Neighbors(Vertex<T> vertex) {
        return vertex.Neighbors();
    }

    public void Clear() {
        _adjacencyList.Clear();
    }

    /// <summary>Represents a Vertex node in a Graph.</summary>
    public class Vertex<V> {
        /// <summary>The data held by the Vertex.</summary>
        public V Data { get; private set; }
        /// <summary>The number of adjacent vertices to this one.</summary>
        public int CountAdjacent { get { return _edgeList.Count; } }
        /// <summary>The list of adjacent Vertices.</summary>
        private readonly List<Edge> _edgeList = new List<Edge>();

        /// <summary>Constructs the Vertex with the initial value.</summary>
        public Vertex(V initialValue) {
            Data = initialValue;    
        }

        /// <summary>Adds an Edge from this Vertex to the provided Vertex.</summary>
        /// <param name="destination">The Vertex being made adjacent to this one.</param>
        public void AddEdge(Vertex<T> destination) {
            _edgeList.Add(new Edge(destination));
        }

        /// <summary>Remove all edges from this Vertex to the specified Vertex.</summary>
        /// <param name="detachFrom">The Vertex to sever ties from.</param>
        public bool RemoveEdge(Vertex<T> detachFrom) {
            if (detachFrom == null) {
                return false;
            }

            // Iterate over an Array of the collection to avoid modifying the collection
            // during enumeration. This we way enumerate the array, and then can modify
            // the collection as needed.
            foreach (var edge in _edgeList.ToArray()) {
                if (edge.AdjacentTo == detachFrom) {
                    _edgeList.Remove(edge);
                }
            }

            return true;
        }

        /// <summary>Provides an enumerable collection of Vertices adjacent to this one.</summary>
        /// <returns>An IEnumerable of adjacent Vertices.</returns>
        public IEnumerable<Vertex<T>> Neighbors() {
            var neighborList = new List<Vertex<T>>();
            foreach (var edge in _edgeList) {
                neighborList.Add(edge.AdjacentTo);
            }

            return neighborList;
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
