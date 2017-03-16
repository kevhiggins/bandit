using UnityEngine.Events;

namespace App.GameEvent
{
    class TownEvents : AbstractEvents
    {
        public StringUnityEvent onReportRobbery;
        public UnityEvent onThresholdReached;
        public UnityEvent onReportRobberyModulus;

        public static void OnReportRobbery(string goldAmount)
        {
            MassivelyInvokeString<TownEvents>("onReportRobbery", goldAmount);
        }

        public static void OnThresholdReached()
        {
            MassivelyInvoke<TownEvents>("onThresholdReached");
        }

        public static void OnReportRobberyModulus()
        {
            MassivelyInvoke<TownEvents>("onReportRobberyModulus");
        }
    }
}