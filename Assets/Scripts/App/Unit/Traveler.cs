using App.Graph;
using App;
using App.GameEvent;
using App.Location;
using UnityEngine;

namespace App.Unit
{
    public class Traveler : AbstractUnit
    {
        public Town SourceTown { get; set; }
        public int goldValue = 10;

        public StringUnityEvent OnRobbed;

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

        public int Robbed()
        {
            var gold = goldValue.ToString();
            Despawn();
            if (OnRobbed != null)
            {
                OnRobbed.Invoke(gold);
            }
            TravelerEvents.OnRobbed(gold);

            return goldValue;
        }

        public void MoveToTown(Town town)
        {
            destinationTown = town;
            var destinationWaypoint = destinationTown.gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
            var endNode = GameManager.instance.nodeFinder.FindAdapter(destinationWaypoint);
            GetComponent<GraphNavigator>().MoveToNode(endNode);
        }
    }
}
