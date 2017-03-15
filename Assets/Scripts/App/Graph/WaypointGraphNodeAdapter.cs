using System.Collections.Generic;
using GraphPathfinding;

namespace App.Graph
{
    public class WaypointGraphNodeAdapter : AbstractGameObjectGraphNode
    {
        private WaypointNodeFinder nodeFinder;
        private WayPoint waypoint;
        private List<IGraphNode> neighbors;

        public WaypointGraphNodeAdapter(WaypointNodeFinder nodeFinder, WayPoint waypoint) : base(waypoint.gameObject)
        {
            this.waypoint = waypoint;
            this.nodeFinder = nodeFinder;
        }

        public override List<IGraphNode> FindNeighbors()
        {
            if (neighbors != null)
            {
                return neighbors;
            }

            neighbors = new List<IGraphNode>();
            foreach (var waypointPercent in waypoint.outs)
            {
                neighbors.Add(nodeFinder.FindAdapter(waypointPercent.waypoint));
            }
            return neighbors;
        }
    }
}