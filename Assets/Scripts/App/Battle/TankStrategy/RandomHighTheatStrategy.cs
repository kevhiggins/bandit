using System;
using System.Linq;

namespace App.Battle.TankStrategy
{
    class RandomHighTheatStrategy : ITankStrategy
    {
        public ICombatant ChooseTank(ICombatTeam team)
        {
            var firstCombatant = team.LivingCombatants().OrderByDescending(combatant => combatant.Threat).FirstOrDefault();
            if (firstCombatant == null)
            {
                return null;
            }

            var random = new Random();
            var tankCandidates = team.LivingCombatants().Where(combatant => combatant.Threat == firstCombatant.Threat).ToList();

            return tankCandidates.ElementAt(random.Next(0, tankCandidates.Count));
        }
    }
}
