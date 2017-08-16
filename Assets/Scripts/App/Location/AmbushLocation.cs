using App.GameEvent;
using App.UI;
using App.Unit;
using UnityEngine;

namespace App.Location
{
    
    public class AmbushLocation : AbstractLocation
    {
        public StringUnityEvent onAmbush = new StringUnityEvent();

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!HasWorker)
            {
                return;
            }

            var traveler = collision.gameObject.GetComponent<Traveler>();

            // If we did not collide with a traveler, then return.
            if (traveler == null)
            {
                return;
            }

            Ambush(traveler);
        }

        protected void Ambush(Traveler traveler)
        {
            var goldReceieved = traveler.Robbed();
            var gold = goldReceieved.ToString();

            GameValueRegistry.Instance.SetRegistryValue("last_robbed_amount", gold);

            GameManager.instance.IncreaseScore(goldReceieved);

            onAmbush.Invoke(gold);

            BanditEvents.OnRob(gold);
        }
    }
}