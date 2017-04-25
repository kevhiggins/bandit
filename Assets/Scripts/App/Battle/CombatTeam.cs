using System.Collections.Generic;
using System.Linq;
using App.Battle.TankStrategy;

namespace App.Battle
{
    public class CombatTeam : ICombatTeam
    {
        public ITankStrategy tankStrategy = new RandomHighTheatStrategy();

        public List<ICombatant> Combatants { get; private set; }

        public CombatTeam(List<ICombatant> combatants)
        {
            Combatants = combatants;
        }

        public ICombatant GetTank()
        {
            return tankStrategy.ChooseTank(this);
        }

        public IEnumerable<ICombatant> LivingCombatants()
        {
            return Combatants.Where(combatant => combatant.IsLiving());
        }

        public List<ICombatant> CombatantsByInitiative()
        {
            return LivingCombatants().OrderByDescending(combatant => combatant.Initiative).ToList();
        }
    }
}