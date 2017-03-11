using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Bandit.Graph;
using GraphPathfinding;

namespace Bandit
{
    public class Town : MonoBehaviour
    {
        public float spawnOffset = 0f;
        public float spawnRate = 5f;
        public int robberyThreshold = 2;

        public List<GameObject> travelerTypes = new List<GameObject>();

        private Dictionary<IGraphNode, int> robberyMap = new Dictionary<IGraphNode, int>();

        void Awake()
        {
            // TODO use a coroutine instead
            InvokeRepeating("SpawnTravelers", spawnOffset, spawnRate);
        }

        void SpawnTravelers()
        {
            var travelerGameObject = CreateTraveler();

            var graphNavigator = travelerGameObject.GetComponent<GraphNavigator>();

            var townWaypoint = gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
            var startNode = GameManager.instance.graph.FindAdapter(townWaypoint);
            graphNavigator.SetTargetNode(startNode);

            // Determine destination town, and tell traveler to go there.
            var destinationTown = GameManager.instance.TownManager.GetDifferentTown(this);
            var traveler = travelerGameObject.GetComponent<Traveler>();
            traveler.SourceTown = this;
            traveler.MoveToTown(destinationTown);
        }

        protected GameObject CreateTraveler()
        {
            var travelerIndex = Random.Range(0, travelerTypes.Count);
            var travelerPrefab = travelerTypes.ElementAt(travelerIndex);
            return Instantiate(travelerPrefab, transform.position, Quaternion.identity);
        }

        private void SpawnSoldier()
        {
            Debug.Log("HEWRO");
        }

        public void ReportRobbery(Traveler traveler, Unit.Bandit bandit)
        {
            var robberyNode = bandit.TargetNode;
            if (robberyMap.ContainsKey(robberyNode))
            {
                robberyMap[robberyNode]++;
            }
            else
            {
                robberyMap.Add(robberyNode, 1);
            }

            var totalRobberyCount = robberyMap.Sum(item => item.Value);
            Debug.Log(totalRobberyCount);
            Debug.Log(totalRobberyCount >= robberyThreshold);
            if (totalRobberyCount >= robberyThreshold)
            {
                SpawnSoldier();
            }

        }
    }
}
