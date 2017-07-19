using System;
using System.Collections.Generic;
using App.Graph;
using App.UI;
using App.Unit;
using App.Worker;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App
{

    public delegate void AfterInitHandler();
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public GameObject graphIllustratorChild = null;

        public bool IsInitialized { get; private set; }
        public bool IsPaused { get; private set; }

        [HideInInspector]
        public WaypointNodeFinder nodeFinder;


        private int score;
        //private List<AudioSource> pausedAudioSources = new List<AudioSource>();

        public int Score
        {
            get { return score; }
            private set
            {
                score = value;
                GameValueRegistry.Instance.SetRegistryValue("total_gold", score.ToString());
            }
        }

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
            var bandits = FindObjectsOfType<Unit.Bandit>();
            GameValueRegistry.Instance.SetRegistryValue("total_bandits", bandits.Length.ToString());


            var travelers = FindObjectsOfType<Traveler>();
            GameValueRegistry.Instance.SetRegistryValue("total_travelers", travelers.Length.ToString());
        }

        public void IncreaseScore(int value)
        {
            Score += value;
        }

        public void DecreaseScore(int value)
        {
            Score -= value;
        }

        public void TogglePause()
        {
            if (IsPaused)
            {
                IsPaused = false;
                Resume();
            }
            else
            {
                IsPaused = true;
                Pause();
            }
        }

        public void Pause()
        {
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
                var animator = go.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = false;
                }

                var objectAnimation = go.GetComponent<Animation>();
                if (objectAnimation != null)
                {
                    objectAnimation.enabled = false;
                }

                //var audioSource = go.GetComponent<AudioSource>();
                //if (audioSource != null)
                //{
                //    if (audioSource.isPlaying)
                //    {
                //        audioSource.Pause();
                //        pausedAudioSources.Add(audioSource);
                //    }

                //}

            }
        }

        public void Resume()
        {
            Object[] objects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in objects)
            {
                go.SendMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
                var animator = go.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.enabled = true;
                }

                var objectAnimation = go.GetComponent<Animation>();
                if (objectAnimation != null)
                {
                    objectAnimation.enabled = true;
                }

                //foreach (var audioSource in pausedAudioSources)
                //{
                //    audioSource.Play();
                //}
            }
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

            var waypoints = FindObjectsOfType<WayPoint>();
            nodeFinder = new WaypointNodeFinder(waypoints);

            // Draw the lines of the nodeFinder.

            var illustrator = new GraphIllustrator();
            illustrator.Draw(nodeFinder, waypoints, graphIllustratorChild);

            // TODO switch Bandit to an AppMonoBehavior to get init for free.
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
