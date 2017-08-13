using System;
using UnityEngine.Events;

namespace App.UI.Events
{
    [Serializable]
    public class GlobalEventManager
    {
        public UnityEvent onJobIconMouseEnter = new UnityEvent();
        public UnityEvent onJobIconMouseExit = new UnityEvent();
    }
}