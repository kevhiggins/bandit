using System.Collections.Generic;

namespace GraphPathfinding
{
    public interface IGraphNode
    {
        int Id { get; }
        float X { get; }
        float Y { get; }

        List<IGraphNode> FindNeighbors();
    }
}
