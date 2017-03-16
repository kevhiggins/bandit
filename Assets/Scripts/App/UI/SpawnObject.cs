using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.UI
{
    class SpawnObject : MonoBehaviour
    {
        public void Spawn(GameObject target)
        {
            Spawn(target, 2);
        }

        public void Spawn(GameObject target, int duration)
        {
            var spawnedGameObject = Instantiate(target, gameObject.transform.position, Quaternion.identity);
            if (duration > 0)
            {
                StartCoroutine(Despawn(spawnedGameObject, duration));
            }
        }

        protected IEnumerator Despawn(GameObject instance, int duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(instance);
        }
    }
}