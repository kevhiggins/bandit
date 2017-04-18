using System.Collections.Generic;
using JetBrains.Annotations;

namespace App.Battle
{
    public interface ICombatTeam
    {
        List<ICombatant> Combatants { get; }

        ICombatant GetTank();

        IEnumerable<ICombatant> LivingCombatants();
    }
}
