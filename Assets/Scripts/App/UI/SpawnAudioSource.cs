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

            // Calling this on the game manager, because if this game object is destroyed before the sound is removed, then it never will be.
            GameManager.instance.StartCoroutine(Despawn(spawnedGameObject, duration));
        }


        protected IEnumerator Despawn(GameObject instance, float duration)
        {
            yield return new WaitForSeconds(duration);
            Destroy(instance);
        }
    }
}