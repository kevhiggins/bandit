using System.Collections.Generic;
using System.Linq;

namespace App.Battle
{
    public class CombatTeam : ICombatTeam
    {
        public List<ICombatant> Combatants { get; private set; }

        public CombatTeam(List<ICombatant> combatants)
        {
            Combatants = combatants;
        }

        public ICombatant GetTank()
        {
            return LivingCombatants().OrderByDescending(combatant => combatant.Threat).FirstOrDefault();
        }

        public IEnumerable<ICombatant> LivingCombatants()
        {
            return Combatants.Where(combatant => combatant.IsLiving());
        }
    }
}