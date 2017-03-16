using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace App.GameEvent
{
    public abstract class AbstractEvents : MonoBehaviour
    {
        protected static void MassivelyInvoke<T>(string propertyName) where T : Object
        {
            foreach (var unityEvent in GetUnityEventEnumerator<UnityEvent, T>(propertyName))
            {
                if (unityEvent != null)
                {
                    unityEvent.Invoke();
                }
            }
        }

        protected static void MassivelyInvokeString<T>(string propertyName, string value) where T : Object
        {
            foreach (var unityEvent in GetUnityEventEnumerator<StringUnityEvent, T>(propertyName))
            {
                if (unityEvent != null)
                {
                    unityEvent.Invoke(value);
                }
            }
        }

        protected static IEnumerable<T> GetUnityEventEnumerator<T, TS>(string propertyName) where TS : Object
        {
            var type = typeof(BanditEvents);
            var field = type.GetField(propertyName);

            var eventObjects = FindObjectsOfType<TS>();
            foreach (var eventObject in eventObjects)
            {
                yield return (T) field.GetValue(eventObject);
            }
        }
    }
}