using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace Bandit.Graph
{
    class GameObjectGraphNode : AbstractGameObjectGraphNode
    {
        private List<IGraphNode> neighbors;

        public GameObjectGraphNode(GameObject gameObject, List<IGraphNode> neighbors) : base(gameObject)
        {
            this.neighbors = neighbors;
        }

        public override List<IGraphNode> FindNeighbors()
        {
            return neighbors;
        }
    }
}