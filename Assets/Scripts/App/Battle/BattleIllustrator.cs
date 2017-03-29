using UnityEngine;

namespace App.Battle
{
    public class BattleIllustrator
    {
        public void DrawBattle(ICombatTeam teamA, ICombatTeam teamB)
        {
            DrawTeam(teamA, -10f, 4f, -1.5f);
            DrawTeam(teamB, 8f, 4f, -1.5f);
        }

        private void DrawTeam(ICombatTeam team, float xStart, float yStart, float yOffset)
        {
            var count = 0;
            foreach (var combatant in team.Combatants)
            {
                Object.Instantiate(combatant.DisplayPrefab, new Vector3(xStart, yStart + count * yOffset, 0), Quaternion.identity);
                count++;
            }
        }
    }
}
