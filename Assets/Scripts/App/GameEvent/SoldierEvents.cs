namespace App.GameEvent
{
    class SoldierEvents : AbstractEvents
    {
        public StringUnityEvent onPunish = null;

        public static void OnPunish(string goldAmount)
        {
            MassivelyInvokeString<SoldierEvents>("onPunish", goldAmount);
        }
    }
}
