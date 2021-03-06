﻿using System.Collections;
using UnityEngine;

namespace App.UI
{
    class SpawnParticleEffect : MonoBehaviour
    {
        public void Spawn(GameObject target)
        {
            var spawnedGameObject = Instantiate(target, gameObject.transform.position, target.transform.rotation);
            var particleEffect = spawnedGameObject.GetComponent<ParticleSystem>();
            var duration = particleEffect.main.duration;
            particleEffect.Play();

            GameManager.instance.StartCoroutine(Despawn(spawnedGameObject, duration, particleEffect));
        }

        protected IEnumerator Despawn(GameObject target, float duration, ParticleSystem particleSystem)
        {
            yield return new WaitForSeconds(duration);

            while (particleSystem.IsAlive())
            {
                yield return new WaitForSeconds(duration);
            }

            Destroy(target);
        }

    }
}