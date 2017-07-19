using System.Collections.Generic;

public class Graph<T> {
    private List<Vertex<T>> adjacencyList;

    public Graph() {
        adjacencyList = new List<Vertex<T>>();
    }

    public Graph(int initialSize) {
        if (initialSize > 0) {
            adjacencyList = new List<Vertex<T>>(initialSize);
        }
    }

    private class Vertex<T> {
        public T Data { public get; private set; }

        private List<Edge> edgeList;
    }

    private class Edge {
         public int AdjacentTo { get; set; }
    }
}
