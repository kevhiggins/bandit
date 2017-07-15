using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using App.GameEvent;
using App.Graph;
using App.Unit;
using GraphPathfinding;
using UnityEngine.Events;

namespace App
{
    public class Town : AppMonoBehavior
    {
        public float spawnOffset = 0f;
        public float spawnRate = 5f;
        public int robberyThreshold = 4;
        public int reportModulus = 2;

        public List<GameObject> travelerTypes = new List<GameObject>();
        public List<GameObject> soldierTypes = new List<GameObject>();

        public IGraphNode Node { get; private set; }

        public StringUnityEvent onReportRobbery;
        public UnityEvent onThresholdReached;
        public UnityEvent onReportRobberyModulus;

        private Dictionary<IGraphNode, int> robberyMap = new Dictionary<IGraphNode, int>();
        private float timeSinceLastSpawn = 0f;
        private bool hasStartedSpawn = false;

        new void Awake()
        {
            GameManager.OnAfterInit += () =>
            {
                // TODO use a coroutine instead
                //InvokeRepeating("SpawnTravelers", spawnOffset, spawnRate);

                var townWaypoint = gameObject.transform.parent.gameObject.GetComponent<WayPoint>();
                Node = GameManager.instance.nodeFinder.FindAdapter(townWaypoint);
                if(Node == null)
                {
                    throw new System.Exception("Could not find node associated with town.");
                }
            };
            base.Awake();
        }

        void Update()
        {
            if (IsPaused)
            {
                return;
            }

            timeSinceLastSpawn += Time.deltaTime;
            if (!hasStartedSpawn)
            { 
                if (timeSinceLastSpawn >= spawnOffset)
                {
                    hasStartedSpawn = true;
                    SpawnTraveler();
                    timeSinceLastSpawn = 0;
                }
            }
            else if(timeSinceLastSpawn >= spawnRate)
            {
                SpawnTraveler();
                timeSinceLastSpawn = 0;
            }

        }


        void SpawnTraveler()
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

            var gold = traveler.goldValue.ToString();
            if (onReportRobbery != null)
            {
                onReportRobbery.Invoke(gold);
            }
            TownEvents.OnReportRobbery(gold);

            if (robberyMap.ContainsKey(robberyNode))
            {
                robberyMap[robberyNode]++;
            }
            else
            {
                robberyMap.Add(robberyNode, 1);
            }

            var totalRobberyCount = robberyMap.Sum(item => item.Value);

            if (totalRobberyCount%reportModulus == 0)
            {
                if (onReportRobberyModulus != null)
                {
                    onReportRobberyModulus.Invoke();
                }
                TownEvents.OnReportRobberyModulus();
            }

            if (totalRobberyCount >= robberyThreshold)
            {
                robberyMap.Clear();
                
                SpawnSoldier(bandit.TargetNode);

                if (onThresholdReached != null)
                {
                    onThresholdReached.Invoke();
                }
                TownEvents.OnThresholdReached();
            }

        }
    }
}
