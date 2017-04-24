using RSG;
using UnityEngine;

namespace App.GamePromise
{
    class PromiseTimerBehavior : MonoBehaviour
    {
        private static bool isExceptionHandlerRegistered = false;

        void Awake()
        {
            if (!isExceptionHandlerRegistered)
            {
                Promise.UnhandledException += (sender, args) => { Debug.Log(args.Exception); };
                isExceptionHandlerRegistered = true;
            }
            
        }

        void Update()
        {
            PromiseTimerHelper.Update(Time.deltaTime);
        }
    }
}
