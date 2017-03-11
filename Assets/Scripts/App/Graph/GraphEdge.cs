using System;
using GraphPathfinding;

namespace App.Graph
{
    class GraphEdge
    {
        public IGraphNode NodeA { get; private set; }
        public IGraphNode NodeB { get; private set; }


        public GraphEdge(IGraphNode nodeA, IGraphNode nodeB)
        {
            NodeA = nodeA;
            NodeB = nodeB;
        }

        public static bool operator ==(GraphEdge a, GraphEdge b)
        {
            if (!ReferenceEquals(a, null) && !ReferenceEquals(a, null))
                return a.Equals(b);

            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            return false;
        }

        public static bool operator !=(GraphEdge a, GraphEdge b)
        {
            return !(a == b);
        }

        public override bool Equals(Object obj)
        {
            var otherEdge = obj as GraphEdge;
            if (ReferenceEquals(otherEdge, null))
            {
                return false;
            }

            return (otherEdge.NodeA.Id == NodeA.Id && otherEdge.NodeB.Id == NodeB.Id) ||
                   (otherEdge.NodeB.Id == NodeA.Id && otherEdge.NodeA.Id == NodeB.Id);
        }

        public override int GetHashCode()
        {
            return NodeA.Id ^ NodeB.Id;
        }
    }
}