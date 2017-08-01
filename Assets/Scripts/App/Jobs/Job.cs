using System.Collections.Generic;
using UnityEngine;

namespace App.Jobs
{
    [CreateAssetMenu(menuName = "Game/Job")]
    public class Job : ScriptableObject
    {
        public string title;
        public string description;

        public Cost cost;
        public Reward reward;

        public BanditCollisionAction collisionAction;

        public List<Modifier> modifiers = new List<Modifier>();
    }
}