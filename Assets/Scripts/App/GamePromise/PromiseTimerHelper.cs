using RSG;

namespace App.GamePromise
{
    class PromiseTimerHelper
    {
        private static PromiseTimer instance;

        public static PromiseTimer Instance {
            get { return instance ?? (instance = new PromiseTimer()); }
            set { instance = value; }
        }

        public static void Update(float deltaTime)
        {
            Instance.Update(deltaTime);
        }
    }
}
