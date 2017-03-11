using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace App.Graph
{
    class GraphIllustrator
    {
        private GameObject graphIllustration;
        private HashSet<GraphEdge> edges = new HashSet<GraphEdge>();

        public void Draw(WaypointGraph graph, WayPoint start, GameObject illustratorChild)
        {
            var startNode = graph.FindAdapter(start);

            Traverse(startNode);

            graphIllustration = new GameObject("GraphIllustrator");

            foreach (var edge in edges)
            {
                var child = Object.Instantiate(illustratorChild);
                var lineRenderer = child.GetComponent<LineRenderer>();

                var vectors = new Vector3[2];
                vectors[0] = new Vector3(edge.NodeA.X, edge.NodeA.Y);
                vectors[1] = new Vector3(edge.NodeB.X, edge.NodeB.Y);
                lineRenderer.SetPositions(vectors);
                lineRenderer.sortingOrder = -1;
                child.transform.parent = graphIllustration.transform;
            }
        }

        private void Traverse(IGraphNode node)
        {
            foreach (var neighbor in node.FindNeighbors())
            {
                var edge = new GraphEdge(node, neighbor);
                if (!edges.Contains(edge))
                {
                    edges.Add(edge);
                    Traverse(neighbor);
                }
            }
        }
    }
}