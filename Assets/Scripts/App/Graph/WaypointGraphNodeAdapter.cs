using System.Collections.Generic;
using GraphPathfinding;

namespace App.Graph
{
    public class WaypointGraphNodeAdapter : AbstractGameObjectGraphNode
    {
        private WaypointGraph graph;
        private WayPoint waypoint;

        public WaypointGraphNodeAdapter(WaypointGraph graph, WayPoint waypoint) : base(waypoint.gameObject)
        {
            this.waypoint = waypoint;
            this.graph = graph;
        }

        public override List<IGraphNode> FindNeighbors()
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