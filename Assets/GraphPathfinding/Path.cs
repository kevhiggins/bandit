using System.Collections.Generic;

namespace GraphPathfinding
{
    public class Path
    {
        public List<IGraphNode> nodes;
        public float Cost { get; private set; }
        public IGraphNode DestinationNode { get; private set; }

        public Path(List<IGraphNode> nodes, float cost, IGraphNode destinationNode)
        {
            this.nodes = nodes;
            Cost = cost;
            DestinationNode = destinationNode;
        }
    }
}
