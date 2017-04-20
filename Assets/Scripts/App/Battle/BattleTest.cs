using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Battle
{
    public class BattleTest : MonoBehaviour
    {
        public List<GameObject> travelers = new List<GameObject>();
        public List<GameObject> bandits = new List<GameObject>();

        public List<GameObject> travelerNameTexts = new List<GameObject>();
        public List<GameObject> travelerHpTexts = new List<GameObject>();

        public List<GameObject> banditNameTexts = new List<GameObject>();
        public List<GameObject> banditHpTexts = new List<GameObject>();

        public float delayPerUnitFight = 0.3f;

        private ICombatTeam teamA;
        private ICombatTeam teamB;

        public void Awake()
        {
            teamA = CreateTeam(travelers);
            teamB = CreateTeam(bandits);

            HookupTextFields();

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

        protected void HookupTextFields()
        {
            HookupNameFields(teamA, travelerNameTexts);
            HookupNameFields(teamB, banditNameTexts);

            HookupOnReceiveTextFields(teamA, travelerHpTexts);
            HookupOnReceiveTextFields(teamB, banditHpTexts);
        }

        protected void HookupOnReceiveTextFields(ICombatTeam team, List<GameObject> textFields)
        {
            var index = 0;
            foreach (var unit in team.Combatants)
            {
                var unitText = textFields[index].GetComponent<Text>();
                var initialHpText = unitText.text;

                // Place unit in a temporary variable so that it survives foreach loop scope iteration in closure.
                var activeUnit = unit;

                UnityAction hitCallback = () =>
                {
                    unitText.text = initialHpText.Replace("{hp}", activeUnit.Hp.ToString());
                };
                hitCallback();

                activeUnit.OnReceiveHit.AddListener(hitCallback);

                index++;
            }
        }

        protected void HookupNameFields(ICombatTeam team, List<GameObject> textFields)
        {
            var index = 0;
            foreach (var unit in team.Combatants)
            {
                var unitText = textFields[index].GetComponent<Text>();
                var initialText = unitText.text;
                unitText.text = initialText.Replace("{name}", unit.Name);

                index++;
            }
        }
    }
}