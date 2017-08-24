using UnityEngine;

namespace App.Worker
{
    [CreateAssetMenu(menuName = "Game/BanditWorker")]
    public class BanditWorkerSettings : ScriptableObject
    {
        public string workerName;
        public int stamina = 5;
        public BanditWorkerUISettings uiSettings;
    }
}