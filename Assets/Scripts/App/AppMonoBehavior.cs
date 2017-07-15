using UnityEngine;

namespace App
{
    public class AppMonoBehavior : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        protected virtual void Awake()
        {
            if (GameManager.instance == null)
            {
                GameManager.OnAfterInit += Init;
            }
            else
            {
                Init();
            }
        }

        protected virtual void Init()
        {
        }

        void OnPauseGame()
        {
            IsPaused = true;
        }

        void OnResumeGame()
        {
            IsPaused = false;
        }
    }
}
