using App.GameEvent;
using App.Graph;
using App.UI;
using GraphPathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace App.Unit
{
    public class Bandit : MonoBehaviour
    {
        public GameObject targetWaypoint;
        public StringUnityEvent onPunished = null;
        public StringUnityEvent onRob = null;
        public UnityEvent onDie = null;

        public IGraphNode TargetNode
        {
            get { return  GetComponent<GraphNavigator>().GetTargetNode(); }
        }

        private GraphNavigator graphNavigator;

        private int totalGold = 0;

        public void Init()
        {
            graphNavigator = gameObject.GetComponent<GraphNavigator>();

            // Deferred to Init method to wait for GameManager to be instantiated.
            var waypoint = targetWaypoint.GetComponent<WayPoint>();
            var node = GameManager.instance.nodeFinder.FindAdapter(waypoint);
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
            var goldAmount = totalGold;
            totalGold = 0;
            GameManager.instance.DecreaseScore(goldAmount);
            if (onPunished != null)
            {
                onPunished.Invoke(goldAmount.ToString());
            }
            BanditEvents.OnPunished(goldAmount.ToString());

            Die();

            return goldAmount;
        }

        protected void Rob(Traveler traveler)
        {
            var goldReceieved = traveler.Robbed(true);
            totalGold += goldReceieved;


            var gold = goldReceieved.ToString();

            GameValueRegistry.Instance.SetRegistryValue("last_robbed_amount", gold);

            GameManager.instance.IncreaseScore(goldReceieved);

            if (onRob != null)
            {
                onRob.Invoke(gold);
            }
            
            BanditEvents.OnRob(gold);
        }

        protected void Die()
        {
            Destroy(gameObject);
            if (onDie != null)
            {
                onDie.Invoke();
            }
            BanditEvents.OnDie();
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