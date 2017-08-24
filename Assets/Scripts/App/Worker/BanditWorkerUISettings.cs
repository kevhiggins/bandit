using UnityEngine;

namespace App.Worker
{
    [CreateAssetMenu(menuName = "Game/BanditWorkerUI")]
    public class BanditWorkerUISettings : ScriptableObject
    {
        public Sprite banditTraySprite;
        public Sprite emblemSprite;
    }
}