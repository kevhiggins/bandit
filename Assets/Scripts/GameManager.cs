using System;
using System.Collections.Generic;
using Bandit.Graph;
using MapEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Bandit
{
    class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject travelerGameObject;
        public GameObject scoreBoard;
        public GameObject graphIllustratorChild;
        public float travelerSpawnRate = 5f;

        [HideInInspector]
        public WaypointGraph graph;

        // TODO find a better home for this.
        [HideInInspector]
        public List<Traveler> activeTravelers;

        private Text scoreText;
        private Town[] towns;
        
        private int score = 0;

        private Bandit selectedBandit;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            InitGame();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var mouseClickLayerMask = Layers.GetLayerBitMask(Layers.BanditClicks);
                var hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity, mouseClickLayerMask);
                var banditClicked = false;
                if (hit.collider != null)
                {
                    var targetGameObject = hit.collider.gameObject;
                    if (targetGameObject.transform.parent != null)
                    {
                        var clickedBandit = targetGameObject.transform.parent.GetComponent<Bandit>();
                        if (clickedBandit != null)
                        {
                            banditClicked = true;
                            if (selectedBandit == clickedBandit)
                            {
                                if (selectedBandit != null)
                                {
                                    selectedBandit.SetIsSelected(false);
                                }
                                selectedBandit = null;
                            }
                            else
                            {
                                if (selectedBandit != null)
                                {
                                    selectedBandit.SetIsSelected(false);
                                }
                                selectedBandit = clickedBandit;
                                selectedBandit.SetIsSelected(true);
                            }
                        }
                    }
                }
                // TODO abstract out the bandit selection logic
                if (!banditClicked && selectedBandit != null)
                {
                    selectedBandit.SetIsSelected(false);
                    selectedBandit = null;
                }
            }
            // On left click, try to move the bandit.
            else if (Input.GetMouseButtonDown(1))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var mouseClickLayerMask = Layers.GetLayerBitMask(Layers.MouseClicks);
                var hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity, mouseClickLayerMask);

                // If a clickable node was clicked, then attempt to move to bandit to it.
                if (hit.collider != null)
                {
                    var targetGameObject = hit.collider.gameObject;
                    if (targetGameObject.transform.parent != null)
                    {
                        var waypoint = targetGameObject.transform.parent.GetComponent<WayPoint>();
                        if (waypoint != null)
                        {
                            if (selectedBandit != null)
                            {
                                selectedBandit.MoveToNode(graph.FindAdapter(waypoint));
                            }
                        }
                    }
                }
            }         
        }

        public void IncreaseScore(int value)
        {
            score += value;
            scoreText.text = score.ToString();
        }

        public static GameObject FindChildByName(GameObject parent, string name)
        {
            foreach (Transform transform in parent.transform)
            {
                if (transform.gameObject.name == name)
                {
                    return transform.gameObject;
                }
            }

            throw new Exception("Could not find game object under parent game object `" + parent.name + "` with name " + name);
        }


        void InitGame()
        {
            activeTravelers = new List<Traveler>();
            towns = FindObjectsOfType<Town>();

            // TODO use a coroutine instead
            InvokeRepeating("SpawnMerchants", 0f, travelerSpawnRate);

            var scoreBoardInstance = Instantiate(scoreBoard);
            scoreText = FindChildByName(scoreBoardInstance, "Score").GetComponent<Text>();

            // Get a single waypoint, and create a waypoint graph.
            var startWaypoint = FindObjectOfType<WayPoint>();
            graph = new WaypointGraph(startWaypoint);

            // Draw the lines of the graph.
            var illustrator = new GraphIllustrator();
            illustrator.Draw(graph, startWaypoint, graphIllustratorChild);

            foreach (var bandit in FindObjectsOfType<Bandit>())
            {
                bandit.Init();
            }
        }

        void SpawnMerchants()
        {
            foreach (var town in towns)
            {
                var townPosition = town.transform.position;
                var townWaypoint = town.gameObject.transform.parent.gameObject.GetComponent<WayPoint>();

                // Spawn the traveler, and point them towards their starting town. They will continue to randomly move through the graph.
                var travelerInstance = Instantiate(travelerGameObject, townPosition, Quaternion.identity);
                var traveler = travelerInstance.GetComponent<Traveler>();
                var travelerWaypoint = travelerInstance.GetComponent<WaypointTraverser>();
                travelerWaypoint.target = townWaypoint;

                activeTravelers.Add(traveler);
            }
        }

        public Town[] GetTowns()
        {
            return towns;
        }
    }
}
