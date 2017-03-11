using UnityEditor;
using UnityEngine;

namespace App
{
    public class MouseRayHit
    {
        private RaycastHit hit;
        private bool hasRayHit;
        private bool hasChecked;

        /**
         * Invalidate any ray hit information.
         */
        public void Clear()
        {
            hasChecked = false;
        }

        /**
         * Check if the mouse has a ray hit in the physics world.
         */
        public bool HasRayHit()
        {
            CheckHit();
            return hasRayHit;
        }

        /**
         * Return the ray hit.
         */
        public RaycastHit GetRayHit()
        {
            CheckHit();
            return hit;
        }

        private void CheckHit()
        {
            if (!hasChecked)
            {
                Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                hasRayHit = Physics.Raycast(worldRay, out hit);
            }
        }
    }
}
