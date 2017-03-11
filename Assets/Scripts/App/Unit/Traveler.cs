﻿using App.Graph;
using App;
using UnityEngine;

namespace App.Unit
{
    public class Traveler : Unit
    {
        public Town SourceTown { get; set; }
        public int goldValue = 10;
        private Town destinationTown;

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Break out early, if no town found.
            var town = collision.gameObject.GetComponent<Town>();
            if (town == null)
            {
                return;
            }

            // If the traveler moves into a town after they've left their spawning town, then despawn them.
            if (town == destinationTown)
            {
                Despawn();
            }
        }

        public int GetRobbed(Bandit bandit)
        {
            SourceTown.ReportRobbery(this, bandit);
            Despawn();
            return goldValue;
        }

        public void MoveToTown(Town town)
        {
            destinationTown = town;
            var destinationWaypoint = destinationTown.gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
            var endNode = GameManager.instance.graph.FindAdapter(destinationWaypoint);
            GetComponent<GraphNavigator>().MoveToNode(endNode);
        }
    }
}
