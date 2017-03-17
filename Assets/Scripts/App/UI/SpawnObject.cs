using UnityEngine;

namespace App.UI
{
    class SpawnObject : MonoBehaviour
    {
        public void Spawn(GameObject target)
        {
            Instantiate(target, gameObject.transform.position, Quaternion.identity);
        }
    }
}