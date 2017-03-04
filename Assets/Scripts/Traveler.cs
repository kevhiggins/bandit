using Bandit;
using Bandit.Graph;
using UnityEngine;

namespace Bandit
{
    public class Traveler : MonoBehaviour
    {
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

        public void Rob()
        {
            GameManager.instance.IncreaseScore(10);
            Despawn();
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
