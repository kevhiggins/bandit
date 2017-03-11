using UnityEngine;

namespace App
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager;

        // Use this for initialization
        void Start()
        {
            if (GameManager.instance == null)
            {
                Instantiate(gameManager);
            }
        }
    }
}