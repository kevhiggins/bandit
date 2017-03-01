using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace Bandit.Graph
{
    class GraphIllustrator
    {
        private GameObject graphIllustration;
        private HashSet<GraphEdge> edges = new HashSet<GraphEdge>();
        private GameObject illustratorChild;

        public void Draw(WaypointGraph graph, WayPoint start, GameObject illustratorChild)
        {
            var startNode = graph.FindAdapter(start);
            this.illustratorChild = illustratorChild;
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

//        void OnPostRender()
//        {
//            GL.PushMatrix();
//            material.SetPass(0);
//            GL.LoadOrtho();
//            GL.Begin(GL.LINES);
//            GL.Color(Color.red);
//
//            var screenScale = new Vector3(1f / Screen.width, 1f / Screen.height, 0);
//            foreach (var edge in edges)
//            {
//                var pointA = Camera.main.WorldToScreenPoint(new Vector3(edge.NodeA.X, edge.NodeA.Y, 0f));
//                var pointB = Camera.main.WorldToScreenPoint(new Vector3(edge.NodeB.X, edge.NodeB.Y));
//
//                pointA.Scale(screenScale);
//                pointB.Scale(screenScale);
//
//                pointA.z = .5f;
//                pointB.z = .5f;
//
//                GL.Vertex(pointA);
//                GL.Vertex(pointB);
//            }
//
//            GL.End();
//            GL.PopMatrix();
//        }
    }
}