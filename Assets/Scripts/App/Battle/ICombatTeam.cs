using System.Collections.Generic;

namespace App.Battle
{
    public interface ICombatTeam
    {
        List<ICombatant> Combatants { get; }
    }
}
