using System.Collections;
using App.GameEvent;
using App.Graph;
using GraphPathfinding;
using UnityEngine;

namespace App.Unit
{
    public class Soldier : AbstractUnit, ISoldier
    {
        public Town SourceTown { get; private set; }
        public float patrolDelay = 2f;
        public int movementRange = 1;

        public StringUnityEvent OnPunish;

        private GraphNavigator graphNavigator;

        void Awake()
        {
            graphNavigator = GetComponent<GraphNavigator>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var bandit = collision.gameObject.GetComponent<Bandit>();

            // If we did not collide with a traveler, then return.
            if (bandit == null)
            {
                return;
            }

            Punish(bandit);
        }

        protected void Punish(Bandit bandit)
        {
            var goldAmount = bandit.Punished();
            var gold = goldAmount.ToString();
            if (OnPunish != null)
            {
                OnPunish.Invoke(gold);
            }
            
            SoldierEvents.OnPunish(gold);
        }

        public void PlaceInTown(Town town)
        {
            transform.position = town.transform.position;
            SourceTown = town;
            graphNavigator.SetTargetNode(SourceTown.Node);
        }

        public void PatrolToNode(IGraphNode targetNode)
        {
            graphNavigator.MoveToNode(targetNode, () =>
            {
                StartCoroutine(PathToSourceTownAfterWait(patrolDelay));
            }, movementRange + 2);
        }

        protected IEnumerator PathToSourceTownAfterWait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            graphNavigator.MoveToNode(SourceTown.Node, Despawn);
        }
    }
}
