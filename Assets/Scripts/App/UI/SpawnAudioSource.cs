using System.Collections;
using UnityEngine;

namespace App.UI
{
    class SpawnAudioSource : MonoBehaviour
    {
        public void Spawn(GameObject target)
        {
            var spawnedGameObject = Instantiate(target, gameObject.transform.position, Quaternion.identity);
            var audioSource = spawnedGameObject.GetComponent<AudioSource>();
            var duration = audioSource.clip.length;
            audioSource.Play();

            StartCoroutine(Despawn(spawnedGameObject, duration));
        }

        protected IEnumerator Despawn(GameObject instance, float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(instance);
        }
    }
}