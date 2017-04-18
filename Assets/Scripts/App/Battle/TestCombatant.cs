using UnityEngine;
using UnityEngine.Events;

namespace App.Battle
{
    public class TestCombatant : MonoBehaviour, ICombatant
    {
        public int hp = 6;
        public int attackPower = 2;
        public int threat = 1;
        public int initiative = 1;
        public UnityEvent onAttack = null;


        public int Hp { get; private set; }

        public int AttackPower { get; private set; }

        public int Threat { get; private set; }

        public int Initiative { get; private set; }

        public GameObject DisplayPrefab { get; private set; }

        public void Init()
        {
            Hp = hp;
            AttackPower = attackPower;
            Threat = threat;
            DisplayPrefab = gameObject;
            Initiative = initiative;
        }

        public void Attack(ICombatant target)
        {
            if (onAttack != null)
            {
                onAttack.Invoke();
            }
            target.ReceiveHit(this);
        }

        public void ReceiveHit(ICombatant attacker)
        {
            hp -= attacker.AttackPower;
        }

        public bool IsLiving()
        {
            return Hp > 0;
        }
    }
}