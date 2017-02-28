using System.Collections.Generic;
using GraphPathfinding;

namespace Bandit.Graph
{
    public class WaypointGraphNodeAdapter : IGraphNode
    {
        private WaypointGraph graph;
        private WayPoint waypoint;

        public WaypointGraphNodeAdapter(WaypointGraph graph, WayPoint waypoint)
        {
            this.waypoint = waypoint;
            this.graph = graph;
        }

        public int Id
        {
            get { return waypoint.GetInstanceID(); }
        }

        public float X
        {
            get { return waypoint.transform.position.x; }
        }

        public float Y
        {
            get { return waypoint.transform.position.y; }
        }

        public List<IGraphNode> FindNeighbors()
        {
            var neighbors = new List<IGraphNode>();

            foreach (var waypointPercent in waypoint.outs)
            {
                neighbors.Add(graph.FindAdapter(waypointPercent.waypoint));
            }
            return neighbors;
        }
    }
}