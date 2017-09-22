using App.UI.Events;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace App
{
    public class AppMonoBehavior : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        [Inject]
        private InputManager inputManager;

        private IObservable<bool> mouseOverStream;

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

        protected IObservable<bool> MouseOverStream()
        {
            if (mouseOverStream != null)
                return mouseOverStream;

            // Streams a true value when the mouse enters the location.
            var mouseEnterStream = this.OnMouseEnterAsObservable().Select(_ => true);

            // Streams a false value when the mouse exists the location
            var mouseExitStream = this.OnMouseExitAsObservable().Select(_ => false);

            // Streams boolean values for whehter or not the mouse is over this location.
            return mouseOverStream = mouseEnterStream.Merge(mouseExitStream);
        }

        protected IObservable<bool> RightClickOverStream()
        {
            var rightMouseUpStream = inputManager.RightMouseUpStream();
            return MouseOverStream().CombineLatest(rightMouseUpStream.DistinctUntilChanged(), (isMouseOver, isMouseUp) => isMouseOver && isMouseUp).DistinctUntilChanged();
        }
    }
}
