using MapEditor;
using UnityEngine;

namespace Bandit
{
    public delegate void TownReached(Merchant merchant, Town town);

    public class Merchant : MonoBehaviour
    {
        public event TownReached OnTownReached = (merchant, town) => { };
        private bool hasLeftTown = false;

        void Update()
        {
            // If a merchant enters a town after leaving their start town, then trigger the OnTownReached event.
            var merchantBounds = gameObject.GetComponent<SpriteRenderer>().bounds;
            var inAnyTown = false;
            foreach(var town in GameManager.instance.GetTowns())
            {
                var townBounds = town.GetComponent<SpriteRenderer>().bounds;
                if (merchantBounds.Intersects(townBounds))
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
    }
}
