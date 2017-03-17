namespace App.GameEvent
{
    class TravelerEvents : AbstractEvents
    {
        public StringUnityEvent onRobbed = null;

        public static void OnRobbed(string goldAmount)
        {
            MassivelyInvokeString<TravelerEvents>("onRobbed", goldAmount);
        }
    }
}
