using System.Collections.Generic;

public class Graph<T> {
    private readonly List<Vertex<T>> _adjacencyList;
    public IEnumerable<Vertex<T>> AdjacencyList { get { return _adjacencyList; } }

    public Graph() {
        _adjacencyList = new List<Vertex<T>>();
    }

    public Graph(int initialSize) {
        if (initialSize > 0) {
            _adjacencyList = new List<Vertex<T>>(initialSize);
        }
    }

    public void AddVertex(T toAdd) {
        _adjacencyList.Add(new Vertex<T>(toAdd));
    }

    public bool AddEdge(T source, T destination) {
        if (source == null || destination == null) {
            return false;
        }

        return true;
    }

    public class Vertex<V> {
        public V Data { get; private set; }
        private readonly List<Edge> edgeList = new List<Edge>();

        public Vertex(V initialValue) {
            Data = initialValue;    
        }
    }

    public class Edge {
         public T AdjacentTo { get; set; }
    }
}
