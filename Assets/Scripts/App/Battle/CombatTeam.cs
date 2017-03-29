using System.Collections.Generic;

namespace App.Battle
{
    public class CombatTeam : ICombatTeam
    {
        public List<ICombatant> Combatants { get; private set; }

        public CombatTeam(List<ICombatant> combatants)
        {
            Combatants = combatants;
        }
    }
}