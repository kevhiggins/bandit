using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace App.GameEvent
{
    class BanditEvents : AbstractEvents
    {
        public StringUnityEvent onRob;
        public StringUnityEvent onPunished;

        public static void OnRob(string goldAmount)
        {           
            MassivelyInvokeString<BanditEvents>("onRob", goldAmount);
        }

        public static void OnPunished(string goldAmount)
        {
            MassivelyInvokeString<BanditEvents>("onPunished", goldAmount);
        }
    }
}