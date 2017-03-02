using System;
using System.Collections.Generic;
using UnityEngine;


namespace Bandit.Graph
{
    public class WaypointGraph
    {
        Dictionary<int, WaypointGraphNodeAdapter> waypointNodeAdapters;

        public WaypointGraph(WayPoint startWayPoint)
        {
            waypointNodeAdapters = new Dictionary<int, WaypointGraphNodeAdapter>();
            VisitNewNodes(startWayPoint);
        }

        private void VisitNewNodes(WayPoint waypoint)
        {
            foreach (var waypointPercent in waypoint.outs)
            {
                var neighbor = waypointPercent.waypoint;
                var neighborId = neighbor.gameObject.GetInstanceID();
                // If we have not visited the current waypoint.
                if (!waypointNodeAdapters.ContainsKey(neighborId))
                {
                    var adapter = new WaypointGraphNodeAdapter(this, neighbor);
                    waypointNodeAdapters.Add(neighborId, adapter);
                    VisitNewNodes(neighbor);
                }
            }
        }

        public WaypointGraphNodeAdapter FindAdapter(WayPoint waypoint)
        {
            return FindAdapter(waypoint.gameObject.GetInstanceID());
        }

        public WaypointGraphNodeAdapter FindAdapter(int id)
        {
            if (!waypointNodeAdapters.ContainsKey(id))
            {
                throw new Exception("Key with ID " + id + " does not exist.");
            }

            return waypointNodeAdapters[id];
        }
    }
}
