using System;

namespace App.Jobs
{
    public class BanditCollisionAction
    {
        [Serializable]
        public class Settings
        {
            public bool enabled = false;
            public bool killTraveler = false;
            public int goldReceived;

            public BanditType modifierType;
            public int goldReceivedModifier;
        }
    }
}