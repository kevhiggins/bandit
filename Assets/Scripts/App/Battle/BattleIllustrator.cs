using System.Collections.Generic;
using UnityEngine;

namespace App.Battle
{
    public class BattleIllustrator
    {
        public void DrawBattle(ICombatTeam teamA, ICombatTeam teamB, List<GameObject> aAnchors, List<GameObject> bAnchors)
        {
            DrawTeam(teamA, aAnchors);
            DrawTeam(teamB, bAnchors);
        }

        private void DrawTeam(ICombatTeam team, List<GameObject> anchors)
        {
            var count = 0;
            foreach (var combatant in team.Combatants)
            {
                combatant.GameObject.transform.position = anchors[count].transform.position;
                count++;
            }
        }
    }
}
