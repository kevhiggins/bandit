using System;

namespace App.Jobs
{
    public class Cost
    {
        [Serializable]
        public class Settings
        {
            public int stamina;
            public int gold;
            public int infamy;
            public int turns;
        }
    }
}