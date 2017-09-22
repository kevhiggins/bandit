using App.Graph;
using App;
using App.GameEvent;
using App.Location;
using RSG;
using UnityEngine;

namespace App.Unit
{
    public class Traveler : AbstractUnit
    {
        public Town SourceTown { get; set; }
        public int goldValue = 10;

        public StringUnityEvent OnRobbed;

        private Town destinationTown;
        private Promise moveToTownPromise;

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

        public int Robbed(bool despawn)
        {
            var gold = goldValue.ToString();

            if (despawn)
            {
                Despawn();
            }

            if (OnRobbed != null)
            {
                OnRobbed.Invoke(gold);
            }
            TravelerEvents.OnRobbed(gold);

            return goldValue;
        }

        public override void Despawn()
        {
            base.Despawn();
            if (moveToTownPromise == null) return;
            moveToTownPromise.Resolve();
            moveToTownPromise = null;
        }

        public IPromise MoveToTown(Town town)
        {
            destinationTown = town;
            var destinationWaypoint = destinationTown.gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
            var endNode = GameManager.instance.nodeFinder.FindAdapter(destinationWaypoint);
            GetComponent<GraphNavigator>().MoveToNode(endNode);
            return moveToTownPromise = new Promise();
        }
    }
}
