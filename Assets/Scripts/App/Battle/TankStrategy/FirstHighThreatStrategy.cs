using System.Linq;

namespace App.Battle.TankStrategy
{
    public class FirstHighThreatStrategy : ITankStrategy
    {
        public ICombatant ChooseTank(ICombatTeam team)
        {
            return team.LivingCombatants().OrderByDescending(combatant => combatant.Threat).FirstOrDefault();
        }
    }
}
