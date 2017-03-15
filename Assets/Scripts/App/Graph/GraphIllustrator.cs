using System.Collections.Generic;
using System.Linq;
using GraphPathfinding;
using UnityEngine;

namespace App.Graph
{
    class GraphIllustrator
    {
        private GameObject graphIllustration;
        private HashSet<GraphEdge> edges = new HashSet<GraphEdge>();

        private HashSet<int> visitedIDs;

        public void Draw(WaypointNodeFinder nodeFinder, WayPoint[] waypoints, GameObject illustratorChild)
        {
            visitedIDs = new HashSet<int>();

            foreach (var waypoint in waypoints)
            {
                if (!visitedIDs.Contains(waypoint.gameObject.GetInstanceID()))
                {
                    var node = nodeFinder.FindAdapter(waypoint);
                    Traverse(node);
                }
            }

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
            visitedIDs.Add(node.Id);
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