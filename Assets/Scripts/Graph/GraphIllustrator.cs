using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace Bandit.Graph
{
    class GraphIllustrator : MonoBehaviour
    {
        private GameObject graphIllustration;
        private HashSet<GraphEdge> edges = new HashSet<GraphEdge>();
        private Material material;

        public void Draw(WaypointGraph graph, WayPoint start, Material material)
        {
            var startNode = graph.FindAdapter(start);
            Traverse(startNode);
            this.material = material;
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

        void OnPostRender()
        {
            //            foreach (var edge in edges)
            //            {
            //                GL.Begin(GL.LINES);
            //                GL.Color(new Color(0f, 0f, 0f, 1f));
            //                GL.Vertex3(edge.NodeA.X, edge.NodeA.Y, 0f);
            //                GL.Vertex3(edge.NodeB.X, edge.NodeB.Y, 0f);
            //                GL.End();
            //            }
            GL.PushMatrix();
            material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);
            GL.Vertex3(-1f, -1f, 0f);
//            GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
            GL.Vertex3(1f, 1f, 0f);
            GL.End();
            GL.PopMatrix();
        }
    }
}
