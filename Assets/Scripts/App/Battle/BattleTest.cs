using System.Collections.Generic;
using UnityEngine;

namespace App.Battle
{
    public class BattleTest : MonoBehaviour
    {
        public List<GameObject> travelers = new List<GameObject>();
        public List<GameObject> bandits = new List<GameObject>();
        public float delayPerUnitFight = 0.3f;

        private ICombatTeam teamA;
        private ICombatTeam teamB;

        public void Awake()
        {
            teamA = CreateTeam(travelers);
            teamB = CreateTeam(bandits);

            // Display the battle on the screen.
            var battleIllustrator = new BattleIllustrator();
            battleIllustrator.DrawBattle(teamA, teamB);

            // Initiate the battle.
//            var battleDirector = new BattleDirector();
//            battleDirector.Battle(teamA, teamB);
        }

        private ICombatTeam CreateTeam(List<GameObject> combatantGameObjects)
        {
            List<ICombatant> combatants = new List<ICombatant>();
            foreach (var combatantGameObject in combatantGameObjects)
            {
                var combatant = combatantGameObject.GetComponent<TestCombatant>();
                combatant.Init();
                combatants.Add(combatant);
            }

            return new CombatTeam(combatants);
        }

        public void Fight()
        {
            var battleDirector = new BattleDirector(this);
            battleDirector.Battle(teamA, teamB);
        }
    }
}