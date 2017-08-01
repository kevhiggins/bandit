using System;

namespace App.Jobs
{
    [Serializable]
    public class BanditCollisionAction
    {
        public bool killTraveler = false;
        public int goldReceived;
        
        public BanditType modifierType;
        public int goldReceivedModifier;
    }
}