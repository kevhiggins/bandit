using System;
using System.Collections.Generic;
using System.Linq;
using App.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace App.Battle
{
    public class BattleManager : MonoBehaviour
    {
        public List<GameObject> travelers = new List<GameObject>();
        public List<GameObject> bandits = new List<GameObject>();

        public List<GameObject> travelerInfoBoxes = new List<GameObject>();
        public List<GameObject> banditInfoBoxes = new List<GameObject>();

        public List<GameObject> travelerNameTexts = new List<GameObject>();
        public List<GameObject> travelerHpTexts = new List<GameObject>();

        public List<GameObject> banditNameTexts = new List<GameObject>();
        public List<GameObject> banditHpTexts = new List<GameObject>();

        public List<GameObject> travelerAnchors = new List<GameObject>();
        public List<GameObject> banditAnchors = new List<GameObject>();

        public bool testBattle = false;

        public float delayPerUnitFight = 0.3f;
        public float delayPerTeamFight = 0.5f;

        public UnityEvent onBattleStart = new UnityEvent();
        public UnityEvent onAttackStart = new UnityEvent();
        public UnityEvent onAttackEnd = new UnityEvent();
        public UnityEvent onBattleEnd = new UnityEvent();
        public UnityEvent onBattleSuccess = new UnityEvent();
        public UnityEvent onBattleFail = new UnityEvent();
        public UnityEvent onEscape = new UnityEvent();

        private ICombatTeam teamA;
        private ICombatTeam teamB;

        public int RoundNumber {
            get { return roundNumber; }
            set
            {
                roundNumber = value;
                GameValueRegistry.Instance.SetRegistryValue("current_battle_round", roundNumber.ToString());
            } 
        }
        private int roundNumber;

        public void Awake()
        {
            if (testBattle)
            {
                StartBattle(CreateTeam(travelers), CreateTeam(bandits));
            }
        }

        public void StartBattle(ICombatTeam team1, ICombatTeam team2)
        {
            teamA = team1;
            teamB = team2;

            RoundNumber++;

            HookupTextFields();

            // Display the battle on the screen.
            var battleIllustrator = new BattleIllustrator();
            battleIllustrator.DrawBattle(teamA, teamB, travelerAnchors, banditAnchors);

            onBattleStart.Invoke();
        }

        private ICombatTeam CreateTeam(List<GameObject> combatantGameObjects)
        {
            List<ICombatant> combatants = new List<ICombatant>();
            foreach (var combatantGameObject in combatantGameObjects)
            {
                var combatant = combatantGameObject.GetComponent<Combatant>();
                combatant.Init();
                combatants.Add(combatant);
            }

            return new CombatTeam(combatants);
        }

        public void Fight(Button fightButton)
        {
            fightButton.interactable = false;
            var battleDirector = new BattleDirector(delayPerUnitFight, delayPerTeamFight);

            onAttackStart.Invoke();

            battleDirector.Battle(teamA, teamB).Done(() =>
            {
                fightButton.interactable = true;
                RoundNumber++;
                onAttackEnd.Invoke();


                if (!teamA.HasLiving() || !teamB.HasLiving())
                {
                    onBattleEnd.Invoke();
                }

                if (!teamA.HasLiving())
                {
                    Success();
                }
                else if(!teamB.HasLiving())
                {
                    Fail();
                }
            });
        }

        public void Escape()
        {
            onEscape.Invoke();
        }

        protected void Success()
        {
            onBattleSuccess.Invoke();
        }

        protected void Fail()
        {
            onBattleFail.Invoke();
        }

        protected void HookupTextFields()
        {
            DestroyUnusedInfoBoxes(teamA, travelerInfoBoxes);
            DestroyUnusedInfoBoxes(teamB, banditInfoBoxes);

            HookupNameFields(teamA, travelerNameTexts);
            HookupNameFields(teamB, banditNameTexts);

            HookupOnReceiveTextFields(teamA, travelerHpTexts);
            HookupOnReceiveTextFields(teamB, banditHpTexts);
        }

        protected void DestroyUnusedInfoBoxes(ICombatTeam team, List<GameObject> infoBoxes)
        {
            var index = 0;
            foreach (var infoBox in infoBoxes)
            {
                if (team.Combatants.ElementAtOrDefault(index) == null)
                {
                    Destroy(infoBox);
                }
                index++;
            }
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
                    unitText.text = initialHpText
                        .Replace("{hp}", activeUnit.Hp.ToString())
                        .Replace("{maxhp}", activeUnit.MaxHp.ToString());
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