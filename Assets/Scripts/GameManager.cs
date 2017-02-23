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
        private List<Route> routes;
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
            var endedRoutes = new List<Route>();
            foreach (var route in routes)
            {
                route.Update();
                if (route.HasReachedDestination())
                {
                    endedRoutes.Add(route);
                }
            }

            foreach (var endedRoute in endedRoutes)
            {
                Destroy(endedRoute.GetMerchant().gameObject);
                routes.Remove(endedRoute);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                var scoredRoutes = new List<Route>();

                foreach (var route in routes)
                {

                    var merchantSpriteRenderer = route.GetMerchant().GetComponent<SpriteRenderer>();

                    if (merchantSpriteRenderer.bounds.IntersectRay(mouseRay))
                    {
                        scoredRoutes.Add(route);
                    }
                }

                foreach (var scoredRoute in scoredRoutes)
                {
                    Destroy(scoredRoute.GetMerchant().gameObject);
                    routes.Remove(scoredRoute);
                    score += 10;
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
            routes = new List<Route>();
            towns = FindObjectsOfType<Town>();

            // TODO use a coroutine instead
            InvokeRepeating("SpawnMerchants", 0f, 5f);

            var scoreBoardInstance = Instantiate(scoreBoard);
            scoreText = FindChildByName(scoreBoardInstance, "Score").GetComponent<Text>();
        }

        void SpawnMerchants()
        {
            var index = 0;
            foreach (var town in towns)
            {
                var townPosition = town.transform.position;

                // Create a map of merchants with their vectors and target city

                var merchantInstance = Instantiate(merchantGameObject, townPosition, Quaternion.identity);
                var merchant = merchantInstance.GetComponent<Merchant>();

                var destinationTown = towns[(index + 1)%towns.Length];
                var route = new Route(merchant, destinationTown);

                routes.Add(route);

                index++;
            }
        }
    }
}
