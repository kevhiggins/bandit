using Bandit;
using Bandit.Graph;
using UnityEngine;

namespace Bandit
{
    public class Traveler : MonoBehaviour
    {
        public Town SourceTown { get; set; }
        public int goldValue = 10;
        private Town destinationTown;

        // TODO use collisions with town hitboxes
        void Update()
        {

        }

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

        public void Despawn()
        {
            Destroy(gameObject);
        }

        public int GetRobbed(Unit.Bandit bandit)
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
