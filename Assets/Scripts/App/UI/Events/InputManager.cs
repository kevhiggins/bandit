using UniRx;
using UnityEngine;

namespace App.UI.Events
{
    public class InputManager
    {
        private IObservable<bool> rightClickUpStream;

        public IObservable<bool> RightMouseUpStream()
        {
            if (rightClickUpStream != null)
            {
                return rightClickUpStream;
            }

            // Stream of the current state of the right mouse button each frame update.
            var rightMouseStream = Observable.EveryUpdate()
                .Select(_ => Input.GetKey(KeyCode.Mouse1));

            // Only be true on right mouse up after down.
            return rightClickUpStream = rightMouseStream.Pairwise((prev, current) => prev && !current);
        }
    }
}