using System;

namespace App.Jobs
{
    public class Modifier
    {
        [Serializable]
        public class Settings
        {
            public string name;
            public BanditType type;

            public Cost.Settings cost;
            public Reward.Settings reward;
        }
    }
}