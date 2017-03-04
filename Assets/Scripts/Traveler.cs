using MapEditor;
using UnityEngine;

namespace Bandit
{
    public delegate void TownReached(Traveler traveler, Town town);

    public class Traveler : MonoBehaviour
    {
        public event TownReached OnTownReached = (traveler, town) => { };
        private bool hasLeftTown = false;

        // TODO use collisions with town hitboxes
        void Update()
        {
            // If a Traveler enters a town after leaving their start town, then trigger the OnTownReached event.
            var travelerBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
            var inAnyTown = false;
            foreach(var town in GameManager.instance.GetTowns())
            {
                var townBounds = town.GetComponent<SpriteRenderer>().bounds;
                if (travelerBounds.Intersects(townBounds))
                {
                    inAnyTown = true;
                    if (hasLeftTown)
                    {
                        OnTownReached(this, town);
                        break;
                    }
                }
            }
            if (!inAnyTown)
            {
                hasLeftTown = true;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Don't worry about collisions if the traveler hasn't left their starting town..
            if (!hasLeftTown)
            {
                return;
            }

            // Break out early, if no town found.
            var town = collision.gameObject.GetComponent<Town>();
            if (town == null)
            {
                return;
            }

            // If the traveler moves into a town after they've left their spawning town, then despawn them.
            Despawn();
        }

        public void Despawn()
        {
            GameManager.instance.activeTravelers.Remove(this);
            Destroy(gameObject);
        }

        public void Rob()
        {
            GameManager.instance.IncreaseScore(10);
            Despawn();
        }
    }
}
