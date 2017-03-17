using System;
using App.Graph;
using App.UI;
using UnityEngine;
using App.Unit;
using JetBrains.Annotations;

namespace App
{

    public delegate void AfterInitHandler();
    class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject graphIllustratorChild = null;

        public bool IsInitialized { get; private set; }

        [HideInInspector]
        public WaypointNodeFinder nodeFinder;


        private int score;

        public int Score
        {
            get { return score; }
            private set
            {
                score = value;
                GameValueRegistry.SetRegistryValue("total_gold", score.ToString());
            }
        }

        private Bandit selectedBandit;

        public GameValueRegistry GameValueRegistry { get; private set; }

        public TownManager TownManager { get; private set; }

        public static event AfterInitHandler OnAfterInit = () => { };

        void Awake()
        {
            IsInitialized = false;
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
                        var clickedBandit = targetGameObject.transform.parent.GetComponent<Unit.Bandit>();
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
                                selectedBandit.MoveToNode(nodeFinder.FindAdapter(waypoint));
                            }
                        }
                    }
                }
            }

            var bandits = FindObjectsOfType<Unit.Bandit>();
            GameValueRegistry.SetRegistryValue("total_bandits", bandits.Length.ToString());


            var travelers = FindObjectsOfType<Traveler>();
            GameValueRegistry.SetRegistryValue("total_travelers", travelers.Length.ToString());
        }

        public void IncreaseScore(int value)
        {
            Score += value;
        }

        public void DecreaseScore(int value)
        {
            Score -= value;
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
            GameValueRegistry = new GameValueRegistry();      

            var waypoints = FindObjectsOfType<WayPoint>();
            nodeFinder = new WaypointNodeFinder(waypoints);

            // Draw the lines of the nodeFinder.


            // Disabled illustrator, since we don't seem to be using it.
//            var illustrator = new GraphIllustrator();
//            illustrator.Draw(nodeFinder, waypoints, graphIllustratorChild);

            foreach (var bandit in FindObjectsOfType<Bandit>())
            {
                bandit.Init();
            }

            Score = 0;
            OnAfterInit();
            IsInitialized = true;
        }
    }
}
