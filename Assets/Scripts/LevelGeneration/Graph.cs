using System.Collections.Generic;

public class Graph<T> {
    private List<Vertex<T>> adjacencyList;
    public IEnumerable<Vertex<T>> AdjacencyList { public get { return adjacencyList; } }

    public Graph() {
        adjacencyList = new List<Vertex<T>>();
    }

    public Graph(int initialSize) {
        if (initialSize > 0) {
            adjacencyList = new List<Vertex<T>>(initialSize);
        }
    }

    public void AddVertex(T toAdd) {
        adjacencyList.Add(new Vertex<T>(toAdd));
    }

    public bool AddEdge(T source, T destination) {
        if (source == null || destination == null) {
            return false;
        }
    }

    private class Vertex<T> {
        public T Data { public get; private set; }
        private List<Edge> edgeList = new List<Edge>();

        public Vertex(T initialValue) {
            Data = initialValue;    
        }
    }

    private class Edge {
         public T AdjacentTo { get; set; }
    }
}
