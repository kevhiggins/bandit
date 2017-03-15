using System;
using System.Collections.Generic;

namespace App.Graph
{
    public class WaypointNodeFinder
    {
        Dictionary<int, WaypointGraphNodeAdapter> waypointNodeAdapters;

        public WaypointNodeFinder(WayPoint[] waypoints)
        {
            waypointNodeAdapters = new Dictionary<int, WaypointGraphNodeAdapter>();

            foreach (var waypoint in waypoints)
            {
                var adapter = new WaypointGraphNodeAdapter(this, waypoint);
                waypointNodeAdapters.Add(adapter.Id, adapter);
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
