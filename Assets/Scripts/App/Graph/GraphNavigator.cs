using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace App.Graph
{
    public delegate void TargetReachedHandler();

    class GraphNavigator : MonoBehaviour
    {
        public float speed = 1;
        public bool HasReachedTarget { get; private set; }

        public UnityEvent OnStartMove;
        public UnityEvent OnEndMove;



        private IGraphNode targetNode;
        private IGraphNode previousNode;
        private Vector3 targetPosition;
        private Path path;
        private IEnumerator<IGraphNode> pathEnumerator;
        private TargetReachedHandler OnTargetReached;

        void Update()
        {
            if (!HasReachedTarget)
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
                        HasReachedTarget = true;
                        OnEndMove.Invoke();
                        if (OnTargetReached != null)
                        {
                            OnTargetReached();
                            OnTargetReached = null;
                        }
                    }
                }
            }
        }

        // TODO Change to use promises DLL
        public void MoveToNode(IGraphNode node, TargetReachedHandler callback)
        {
            OnTargetReached = callback;
            OnStartMove.Invoke();
            MoveToNode(node);
        }

        public void MoveToNode(IGraphNode node)
        {
            // Find the shortest route to the destination node, and start moving towards it.
            var pathfinder = new AStarPathfinder();
            path = pathfinder.FindPath(GetTravelingNode(), node);
            if (path != null)
            {
                pathEnumerator = path.nodes.GetEnumerator();
                pathEnumerator.MoveNext();
                SetTargetNode(pathEnumerator.Current);
            }
        }

        public void SetTargetNode(IGraphNode node)
        {
            // Set previous node to current node if we aren't already moving.
            if (targetNode == null)
            {
                previousNode = node;
            }
            else
            {
                previousNode = targetNode;
            }

            targetNode = node;
            targetPosition = new Vector3(targetNode.X, targetNode.Y, 0);
            HasReachedTarget = false;
        }

        public IGraphNode GetTargetNode()
        {
            return targetNode;
        }

        protected IGraphNode GetTravelingNode()
        {
            // If HasReachedTarget, return the targetNode, since the bandit is sitting on top of it. Otherwise, create an IGraphNode for the bandit with one directional edges to the target node, and previous node
            if (HasReachedTarget)
            {
                return targetNode;
            }

            var neighbors = new List<IGraphNode> {targetNode, previousNode};

            return new GameObjectGraphNode(gameObject, neighbors);
        }
    }
}