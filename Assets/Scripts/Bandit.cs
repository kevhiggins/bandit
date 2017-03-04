using System.Collections.Generic;
using Bandit.Graph;
using GraphPathfinding;
using UnityEngine;

namespace Bandit
{
    class Bandit : MonoBehaviour
    {
        public GameObject targetWaypoint;
        public float speed = 1;

        private IGraphNode targetNode;
        private IGraphNode previousNode;
        private bool hasReachedTarget;
        private Vector3 targetPosition;
        private Path path;
        private IEnumerator<IGraphNode> pathEnumerator;

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
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed*Time.deltaTime);

                // If the target position has been reached, then stop moving.
                if (transform.position == targetPosition)
                {
                    // If we have a path, and haven't reached the end, then start moving towards the next node.
                    // Otherwise, we've reached the target and can stop.
                    if (pathEnumerator != null && pathEnumerator.MoveNext())
                    {
                        SetTargetNode(pathEnumerator.Current);
                    }
                    else
                    {
                        hasReachedTarget = true;
                    }
                }
            }
        }

        protected void SetTargetNode(IGraphNode node)
        {
            previousNode = targetNode;
            targetNode = node;
            targetPosition = new Vector3(targetNode.X, targetNode.Y, 0);
            hasReachedTarget = false;
        }

        public void MoveToNode(IGraphNode node)
        {
            // Find the shortest route to the destination node, and start moving towards it.
            var pathfinder = new AStarPathfinder();
            path = pathfinder.FindPath(GetBanditNode(), node);
            pathEnumerator = path.nodes.GetEnumerator();
            pathEnumerator.MoveNext();
            SetTargetNode(pathEnumerator.Current);
        }

        protected IGraphNode GetBanditNode()
        {
            // If hasReachedTarget, return the targetNode, since the bandit is sitting on top of it. Otherwise, create an IGraphNode for the bandit with one directional edges to the target node, and previous node

            if (hasReachedTarget)
            {
                return targetNode;
            }

            var neighbors = new List<IGraphNode> {targetNode, previousNode};

            return new GameObjectGraphNode(gameObject, neighbors);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Only look for collisions while the bandit is stationary.
            if (!hasReachedTarget)
            {
                return;
            }

            var traveler = collision.gameObject.GetComponent<Traveler>();

            // If we did not collide with a traveler, then return.
            if (traveler == null)
            {
                return;
            }

            traveler.Rob();
        }
    }
}