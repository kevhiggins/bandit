using System.Collections.Generic;
using UnityEngine;

namespace App.Jobs
{
    [CreateAssetMenu(menuName = "Game/Job")]
    public class JobSettings : ScriptableObject
    {
        public string title;
        public string description;

        public Sprite icon;

        [Tooltip("The worker will be returned to the player when the job cost is paid in full.")]
        public bool resetWorkerWhenPaid = true;

        public Cost.Settings cost;
        public Reward.Settings reward;

        public BanditCollisionAction.Settings collisionAction;

        public List<Modifier.Settings> synergyModifiers = new List<Modifier.Settings>();
    }
}