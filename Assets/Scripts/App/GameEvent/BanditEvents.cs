using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace App.GameEvent
{
    class BanditEvents : MonoBehaviour
    {
        public StringUnityEvent onRob;
        public StringUnityEvent onPunished;

        public static void OnRob(string goldAmount)
        {

            MassivelyInvokeString("onRob", goldAmount);
        }

        public static void OnPunished(string goldAmount)
        {
            MassivelyInvokeString("onPunished", goldAmount);
        }



        protected static void MassivelyInvoke(string propertyName)
        {
            foreach (var unityEvent in GetUnityEventEnumerator<UnityEvent>(propertyName))
            {
                if (unityEvent != null)
                {
                    unityEvent.Invoke();
                }
            }
        }

        protected static void MassivelyInvokeString(string propertyName, string value)
        {
            foreach (var unityEvent in GetUnityEventEnumerator<StringUnityEvent>(propertyName))
            {
                if (unityEvent != null)
                {
                    unityEvent.Invoke(value);
                }
            }
        }

        protected static IEnumerable<T> GetUnityEventEnumerator<T>(string propertyName)
        {
            var type = typeof(BanditEvents);
            var field = type.GetField(propertyName);

            var eventObjects = FindObjectsOfType<BanditEvents>();
            foreach (var eventObject in eventObjects)
            {
                yield return (T)field.GetValue(eventObject);              
            }
        }
    }
}