using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using App.Graph;
using App.Unit;
using GraphPathfinding;

namespace App
{
    public class Town : MonoBehaviour
    {
        public float spawnOffset = 0f;
        public float spawnRate = 5f;
        public int robberyThreshold = 2;

        public List<GameObject> travelerTypes = new List<GameObject>();
        public List<GameObject> soldierTypes = new List<GameObject>();

        public IGraphNode Node { get; private set; }

        private Dictionary<IGraphNode, int> robberyMap = new Dictionary<IGraphNode, int>();

        void Awake()
        {
            GameManager.OnAfterInit += () =>
            {
                // TODO use a coroutine instead
                InvokeRepeating("SpawnTravelers", spawnOffset, spawnRate);

                var townWaypoint = gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
                Node = GameManager.instance.nodeFinder.FindAdapter(townWaypoint);
            };
        }

        void SpawnTravelers()
        {
            var travelerGameObject = CreateTraveler();

            var graphNavigator = travelerGameObject.GetComponent<GraphNavigator>();

            graphNavigator.SetTargetNode(Node);

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

        protected Soldier CreateSoldier()
        {
            var soldierIndex = Random.Range(0, soldierTypes.Count);
            var soldierPrefab = soldierTypes.ElementAt(soldierIndex);
            var soldierGameObject = Instantiate(soldierPrefab);
            var soldier = soldierGameObject.GetComponent<Soldier>();
            soldier.PlaceInTown(this);
            return soldier;
        }

        private void SpawnSoldier(IGraphNode targetNode)
        {
            var soldier = CreateSoldier();
            soldier.PatrolToNode(targetNode);
        }

        public void ReportRobbery(Traveler traveler, Bandit bandit)
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
            if (totalRobberyCount >= robberyThreshold)
            {
                robberyMap.Clear();
                SpawnSoldier(bandit.TargetNode);
            }

        }
    }
}
