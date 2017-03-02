using GraphPathfinding;
using UnityEngine;

namespace Bandit
{
    class Bandit : MonoBehaviour
    {
        public GameObject targetWaypoint;
        public float speed = 1;

        private IGraphNode targetNode;
        private bool hasReachedTarget;
        private Vector3 targetPosition;

        public void Init()
        {
            // Deferred to Init method to wait for GameManager to be instantiated.
            var waypoint = targetWaypoint.GetComponent<WayPoint>();
            var node = GameManager.instance.graph.FindAdapter(waypoint);
            SetTargetNode(node);
        }

        void Update()
        {
            if (!hasReachedTarget)
            {
                // Move the bandit towards the target, and prevent over shooting.
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                // If the target position has been reached, then stop moving.
                if (transform.position == targetPosition)
                {
                    hasReachedTarget = true;
                }
            }
        }

        public void SetTargetNode(IGraphNode node)
        {
            targetNode = node;
            targetPosition = new Vector3(targetNode.X, targetNode.Y, 0);
            hasReachedTarget = false;
        }
    }
}
