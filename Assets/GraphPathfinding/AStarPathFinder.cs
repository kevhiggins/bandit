using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphPathfinding
{
    public delegate float MovementCostFunction(IGraphNode sourceNode, IGraphNode destinationNode);

    public delegate int CostFunction(IGraphNode node);

    public delegate float HeuristicFunction(IGraphNode node, IGraphNode goalNode);

    public delegate bool FoundNodeValidFunction(IGraphNode sourceNode, IGraphNode destinationNode);

    public class AStarPathfinder
    {
        public MovementCostFunction movementCost = ManhattanDistance;
        public HeuristicFunction heuristic = ManhattanDistance;
        public FoundNodeValidFunction foundNodeValid = (sourceNode, destinationNode) => true;

        public bool findNodeAdjacentToDestination = false;

        private Dictionary<IGraphNode, float> nodeCosts = new Dictionary<IGraphNode, float>();
        private Dictionary<IGraphNode, IGraphNode> nodeParents = new Dictionary<IGraphNode, IGraphNode>();

        public Path FindPath(IGraphNode startNode, IGraphNode destinationNode)
        {
            return Run(startNode, destinationNode, 0).first;
        }

        public HashSet<IGraphNode> FindNodesInCostRange(IGraphNode startNode, int cost)
        {
            var oldHeuristic = heuristic;
            heuristic = (a, b) => 0;
            var result = Run(startNode, null, cost).second;
            heuristic = oldHeuristic;
            return result;
        }

        protected void SetNodeCost(IGraphNode node, float cost)
        {
            nodeCosts[node] = cost;
        }

        protected void SetNodeParent(IGraphNode node, IGraphNode parent)
        {
            nodeParents[node] = parent;
        }

        public float GetNodeCost(IGraphNode node)
        {
            return nodeCosts[node];
        }

        public IGraphNode GetNodeParent(IGraphNode node)
        {
            if (!nodeParents.ContainsKey(node))
                return null;
            return nodeParents[node];
        }

        protected Tuple<Path, HashSet<IGraphNode>> Run(IGraphNode sourceNode, IGraphNode destinationNode, int maxCost)
        {
            nodeParents = new Dictionary<IGraphNode, IGraphNode>();
            nodeCosts = new Dictionary<IGraphNode, float>();

            var openNodes = new Dictionary<IGraphNode, float>();
            var closedNodes = new HashSet<IGraphNode>();

            openNodes[sourceNode] = 0;
            SetNodeCost(sourceNode, 0);
            var currentNode = sourceNode;

            var pathFound = false;

            while (openNodes.Any())
            {
                currentNode = DequeueNextItem(openNodes);

                var currentNodeCost = GetNodeCost(currentNode);

                // If a max cost is configured, then use it.
                if (maxCost > 0 && currentNodeCost > maxCost)
                    continue;

                // If we are trying to find a path to a destination, then compare the current node to the destination.
                if (!findNodeAdjacentToDestination && destinationNode != null)
                {
                    // If current node is the destination node, then break out of loop.
                    if (currentNode.Id == destinationNode.Id)
                    {
                        // IF the found node function approves of the node, than we've found the correct node. Otherwise, this node is invalid, so we continue to the next open node.
                        if (foundNodeValid(sourceNode, currentNode))
                        {
                            return new Tuple<Path, HashSet<IGraphNode>>(GeneratePath(currentNode, destinationNode), closedNodes);
                        }
                        continue;
                    }
                }

                closedNodes.Add(currentNode);

                var neighbors = currentNode.FindNeighbors();
                foreach (var neighbor in neighbors)
                {
                    if (findNodeAdjacentToDestination && destinationNode != null && neighbor.Id == destinationNode.Id)
                    {
                        if (foundNodeValid(sourceNode, currentNode))
                            return new Tuple<Path, HashSet<IGraphNode>>(GeneratePath(currentNode, destinationNode), closedNodes);
                        continue;
                    }

                    var movementCostValue = movementCost(currentNode, neighbor);
                    if (movementCostValue == -1)
                    {
                        continue;
                    }

                    var cost = currentNodeCost + movementCostValue;
                    if (openNodes.ContainsKey(neighbor) && cost < GetNodeCost(neighbor))
                        openNodes.Remove(neighbor);
                    if (closedNodes.Contains(neighbor) && cost < GetNodeCost(neighbor))
                        closedNodes.Remove(neighbor);
                    if (!openNodes.ContainsKey(neighbor) && !closedNodes.Contains(neighbor))
                    {
                        SetNodeCost(neighbor, cost);
                        openNodes.Add(neighbor, cost + heuristic(neighbor, destinationNode));
                        SetNodeParent(neighbor, currentNode);
                    }
                }
            }

            return new Tuple<Path, HashSet<IGraphNode>>(null, closedNodes);
        }

        protected Path GeneratePath(IGraphNode currentNode, IGraphNode destinationNode)
        {
            var tmpNode = currentNode;
            var nodeList = new List<IGraphNode>();

            while (tmpNode != null)
            {
                nodeList.Add(tmpNode);
                tmpNode = GetNodeParent(tmpNode);
            }

            // Reverse list so it goes from start node to goal node.
            nodeList.Reverse();
            return new Path(nodeList, GetNodeCost(currentNode), destinationNode);
        }


        protected IGraphNode DequeueNextItem(Dictionary<IGraphNode, float> openList)
        {
            var nextItem = openList.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            openList.Remove(nextItem);
            return nextItem;
        }

        public static float ManhattanDistance(IGraphNode startNode, IGraphNode endNode)
        {
            var dx = Math.Abs(startNode.X - endNode.X);
            var dy = Math.Abs(startNode.Y - endNode.Y);
            var d = 1;
            return d*(dx + dy);
        }
    }
}