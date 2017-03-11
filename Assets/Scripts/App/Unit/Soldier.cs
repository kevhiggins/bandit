using System.Collections;
using System.Collections.Generic;
using App.Graph;
using GraphPathfinding;
using UnityEngine;

namespace App.Unit
{
    public class Soldier : Unit, ISoldier
    {
        public Town SourceTown { get; private set; }

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
            bandit.LoseGold();
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
                StartCoroutine(PathToSourceTownAfterWait(2));
            });
        }

        protected IEnumerator PathToSourceTownAfterWait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            graphNavigator.MoveToNode(SourceTown.Node, Despawn);
        }
    }
}
