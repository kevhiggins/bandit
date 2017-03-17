using UnityEngine;

namespace App
{
    public class AppMonoBehavior : MonoBehaviour
    {
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
    }
}
