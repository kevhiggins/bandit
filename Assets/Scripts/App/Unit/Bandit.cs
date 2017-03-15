﻿using App.GameEvent;
using App.Graph;
using GraphPathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace App.Unit
{
    public class Bandit : MonoBehaviour
    {
        public GameObject targetWaypoint;
        public UnityEvent onPunished;
        public StringUnityEvent onRob;

        public IGraphNode TargetNode
        {
            get { return  GetComponent<GraphNavigator>().GetTargetNode(); }
        }

        private GraphNavigator graphNavigator;

        public void Init()
        {
            graphNavigator = gameObject.GetComponent<GraphNavigator>();

            // Deferred to Init method to wait for GameManager to be instantiated.
            var waypoint = targetWaypoint.GetComponent<WayPoint>();
            var node = GameManager.instance.graph.FindAdapter(waypoint);
            graphNavigator.SetTargetNode(node);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Only look for collisions while the bandit is stationary.
            if (!graphNavigator.HasReachedTarget)
            {
                return;
            }

            var traveler = collision.gameObject.GetComponent<Traveler>();

            // If we did not collide with a traveler, then return.
            if (traveler == null)
            {
                return;
            }

            Rob(traveler);
        }

        public int Punished()
        {
            var goldAmount = GameManager.instance.Score;
            GameManager.instance.DecreaseScore(goldAmount);
            onPunished.Invoke();
            return goldAmount;
        }

        protected void Rob(Traveler traveler)
        {
            var goldReceieved = traveler.GetRobbed(this);
            GameManager.instance.IncreaseScore(goldReceieved);
            var test = goldReceieved.ToString();

            if (onRob != null)
            {
                onRob.Invoke(test);
            }
            
            BanditEvents.OnRob();
        }

        public void Testing(string hi)
        {
            
        }

        public void MoveToNode(IGraphNode node)
        {
            graphNavigator.MoveToNode(node);
        }

        public void SetIsSelected(bool value)
        {
            var selectionAnimationObject = GameManager.FindChildByName(gameObject, "SelectionAnimator");
            var animator = selectionAnimationObject.GetComponent<Animator>();
            animator.SetBool("IsSelected", value);
        }
    }
}