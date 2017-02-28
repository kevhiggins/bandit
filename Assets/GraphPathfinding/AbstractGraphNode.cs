using System.Collections.Generic;

namespace GraphPathfinding
{
    public abstract class AbstractGraphNode : IGraphNode
    {
        public abstract int Id { get; }
        public abstract float X { get; }
        public abstract float Y { get; }

        public IGraphNode ParentNode { get; set; }
        public int TentativeCost { get; set; }

        private List<IGraphNode> neighbors = new List<IGraphNode>();

        public List<IGraphNode> FindNeighbors()
        {
            return neighbors;
        }

        public void AddNeighbor(IGraphNode node)
        {
            neighbors.Add(node);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}