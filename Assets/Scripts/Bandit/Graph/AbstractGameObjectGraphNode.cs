using System.Collections.Generic;
using GraphPathfinding;
using UnityEngine;

namespace Bandit.Graph
{
    public abstract class AbstractGameObjectGraphNode : IGraphNode
    {
        protected GameObject gameObject;

        protected AbstractGameObjectGraphNode(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public int Id
        {
            get { return gameObject.GetInstanceID(); }
        }

        public float X
        {
            get { return gameObject.transform.position.x; }
        }

        public float Y
        {
            get { return gameObject.transform.position.y; }
        }

        public abstract List<IGraphNode> FindNeighbors();
    }
}
