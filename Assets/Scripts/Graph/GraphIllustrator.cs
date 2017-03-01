using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace Bandit.Graph
{
    class GraphIllustrator
    {
        private GameObject graphIllustration;
        private HashSet<GraphEdge> edges = new HashSet<GraphEdge>();
        private Material material;

        public void Draw(WaypointGraph graph, WayPoint start, Material material)
        {
            var startNode = graph.FindAdapter(start);
            Traverse(startNode);
            material.color = Color.white;
            this.material = material;


            graphIllustration = new GameObject("GraphIllustrator");

            foreach (var edge in edges)
            {
                var child = new GameObject("Line");
                var lineRenderer = child.AddComponent<LineRenderer>();
                lineRenderer.material = this.material;
                lineRenderer.startColor = new Color(222f/255f, 184f/255f, 135f/255f);
                lineRenderer.endColor = new Color(222f/255f, 184f/255f, 135f/255f);
                lineRenderer.startWidth = .1f;
                lineRenderer.endWidth = .1f;

                var vectors = new Vector3[2];
                vectors[0] = new Vector3(edge.NodeA.X, edge.NodeA.Y);
                vectors[1] = new Vector3(edge.NodeB.X, edge.NodeB.Y);
                lineRenderer.SetPositions(vectors);
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