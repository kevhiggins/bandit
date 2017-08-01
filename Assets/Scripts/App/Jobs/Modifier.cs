using System;

namespace App.Jobs
{
    [Serializable]
    public class Modifier
    {
        public string name;
        public BanditType type;

        public Cost cost;
        public Reward reward;
    }
}