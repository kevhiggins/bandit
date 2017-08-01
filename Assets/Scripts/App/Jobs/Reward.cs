using System;

namespace App.Jobs
{
    [Serializable]
    public class Reward
    {
        public int stamina;
        public int gold;
        public int infamy;
        public int goldPerTurn;
    }
}