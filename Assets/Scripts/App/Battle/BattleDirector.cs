using System;
using System.Collections.Generic;
using App.GamePromise;
using RSG;

namespace App.Battle
{
    public class BattleDirector
    {
        private float delayPerUnitFight;
        private float delayPerTeamFight;

        public BattleDirector(float delayPerUnitFight, float delayPerTeamFight)
        {
            this.delayPerUnitFight = delayPerUnitFight;
            this.delayPerTeamFight = delayPerTeamFight;
        }

        public IPromise Battle(ICombatTeam teamA, ICombatTeam teamB)
        {
            return TeamAttack(teamA, teamB)
                .Then(() => PromiseTimerHelper.Instance.WaitFor(delayPerTeamFight))
                .Then(() => TeamAttack(teamB, teamA));
        }

        protected IPromise TeamAttack(ICombatTeam teamA, ICombatTeam teamB)
        {
            var promises = new List<Func<IPromise>>();

            // Foreach unit in team A, attack the highest living unit on teamB with the highest threat.
            foreach (var activeUnit in teamA.CombatantsByInitiative())
            {
                var attacker = activeUnit;

                promises.Add(() => UnitAttack(attacker, teamB));
            }

            return Promise.Sequence(promises);
        }

        protected IPromise UnitAttack(ICombatant attacker, ICombatTeam defendingTeam)
        {
            // Get the highest threat enemy unit and attack it.
            var defender = defendingTeam.GetTank();

            // TODO look into breaking out of the group of attacks early if possible.
            // Return an autoresolved promise if no defender is found.
            if (defender == null)
            {
                return Promise.Resolved();
            }

            attacker.Attack(defender);
            return PromiseTimerHelper.Instance.WaitFor(delayPerUnitFight);
        }
    }
}