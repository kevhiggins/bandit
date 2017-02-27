using System;
using System.Collections.Generic;
using MapEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Bandit
{
    class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public GameObject merchantGameObject;
        public GameObject scoreBoard;
        private Text scoreText;


        private Town[] towns;
        private List<Merchant> activeMerchants;
        private int score = 0;

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
                var scoredMerchants = new List<Merchant>();
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                foreach (var activeMerchant in activeMerchants)
                {
                    var merchantSpriteRenderer = activeMerchant.GetComponent<SpriteRenderer>();
                    
                    if (merchantSpriteRenderer.bounds.IntersectRay(mouseRay))
                    {
                        scoredMerchants.Add(activeMerchant);
                    }       
                }

                foreach (var scoredMerchant in scoredMerchants)
                {
                    activeMerchants.Remove(scoredMerchant);
                    score += 10;
                    Destroy(scoredMerchant.gameObject);
                }

            }

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
            activeMerchants = new List<Merchant>();
            towns = FindObjectsOfType<Town>();

            // TODO use a coroutine instead
            InvokeRepeating("SpawnMerchants", 0f, 5f);

            var scoreBoardInstance = Instantiate(scoreBoard);
            scoreText = FindChildByName(scoreBoardInstance, "Score").GetComponent<Text>();
        }

        void SpawnMerchants()
        {
            foreach (var town in towns)
            {
                var townPosition = town.transform.position;
                var townWaypoint = town.gameObject.transform.parent.gameObject.GetComponent<WayPoint>();

                var merchantInstance = Instantiate(merchantGameObject, townPosition, Quaternion.identity);
                var merchant = merchantInstance.GetComponent<Merchant>();
                var merchantWaypoint = merchantInstance.GetComponent<WaypointTraverser>();
                merchantWaypoint.target = townWaypoint;

                // When a merchant reaches a new town, despawn them.
                merchant.OnTownReached += (successfulMerchant, endTown) =>
                {;
                    activeMerchants.Remove(successfulMerchant);
                    Destroy(successfulMerchant.gameObject);
                };

                activeMerchants.Add(merchant);
            }
        }

        public Town[] GetTowns()
        {
            return towns;
        }
    }
}
