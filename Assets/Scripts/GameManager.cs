using System;
using Bandit.Graph;
using UnityEngine;
using UnityEngine.UI;

namespace Bandit
{
    class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject scoreBoard = null;
        public GameObject graphIllustratorChild = null;

        [HideInInspector]
        public WaypointGraph graph;

        private Text scoreText;
        
        private int score = 0;

        private Bandit selectedBandit;

        public TownManager TownManager { get; private set; }


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
            TownManager = new TownManager();            

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


    }
}
