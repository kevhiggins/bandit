using App.Unit;
using UnityEngine;

namespace App
{
    class HudInputManager : MonoBehaviour
    {
        public GameObject banditStartWaypoint = null;
        public GameObject banditPrefab = null;

        public void PurchaseBandit()
        {
            var banditInstance = Instantiate(banditPrefab, banditStartWaypoint.transform.position, Quaternion.identity);
            var bandit = banditInstance.GetComponent<Bandit>();
            bandit.targetWaypoint = banditStartWaypoint;
            bandit.Init();
        }
    }
}
