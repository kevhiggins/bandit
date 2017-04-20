using System.Collections;
using System.Linq;
using UnityEngine;

namespace App.Battle
{
    public class BattleDirector
    {
        public float DelayPerUnitFight { get; private set; }
        private MonoBehaviour coroutineManager;

        public BattleDirector(MonoBehaviour coroutineManager, float delayPerUnitFight = 0.3f)
        {
            DelayPerUnitFight = delayPerUnitFight;
            this.coroutineManager = coroutineManager;
        }

        public void Battle(ICombatTeam teamA, ICombatTeam teamB)
        {
            coroutineManager.StartCoroutine(TeamAttack(teamA, teamB));

            // TODO Add a configure for a wait between teams fighting?

            coroutineManager.StartCoroutine(TeamAttack(teamB, teamA));
        }

        protected IEnumerator TeamAttack(ICombatTeam teamA, ICombatTeam teamB)
        {
            // Foreach unit in team A, attack the highest living unit on teamB with the highest threat.
            foreach (var activeUnit in teamA.LivingCombatants().OrderByDescending(unit => unit.Initiative))
            {
                // Get the highest threat enemy unit and attack it.
                var tank = teamB.GetTank();

                // If there is no tank, then break out of the loop.
                if (tank == null)
                {
                    break;
                }

                activeUnit.Attack(tank);

                // Wait the configured amount of time before starting the next fight.
                yield return new WaitForSeconds(DelayPerUnitFight);
            }
        }
    }
}