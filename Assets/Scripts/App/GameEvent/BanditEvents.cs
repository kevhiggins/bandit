using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace App.GameEvent
{
    class BanditEvents : MonoBehaviour
    {
        // Bandit.Rob() => BanditEvents.OnRob() => Find all BanditEvents instances in the scene (Use awake) and call invoke on the corresponding event.
        public UnityEvent onRob;

        public static void OnRob()
        {

            MassivelyInvoke("onRob");
        }

        protected static void MassivelyInvoke(string propertyName)
        {
            var type = typeof(BanditEvents);
            var field = type.GetField(propertyName);

            var eventObjects = FindObjectsOfType<BanditEvents>();
            foreach (var eventObject in eventObjects)
            {
                var unityEvent = (UnityEvent)field.GetValue(eventObject);
                if (unityEvent != null)
                {
                    unityEvent.Invoke();
                }
            }
        }
    }
}