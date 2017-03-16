using System.Collections;
using UnityEngine;

namespace App.UI
{
    class SpawnParticleEffect : MonoBehaviour
    {
        public void Spawn(GameObject target)
        {
            var spawnedGameObject = Instantiate(target, gameObject.transform.position, Quaternion.identity);
            var particleEffect = spawnedGameObject.GetComponent<ParticleSystem>();
            var duration = particleEffect.main.duration;
            particleEffect.Play();

            StartCoroutine(Despawn(spawnedGameObject, duration));
        }

        protected IEnumerator Despawn(GameObject target, float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(target);
        }

    }
}