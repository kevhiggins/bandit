using App.GamePromise;
using App.UI;
using UnityEngine;
using UnityEngine.Events;

namespace App.Battle
{
    public class Combatant : MonoBehaviour, ICombatant
    {
        public int hp = 6;
        public int attackPower = 2;
        public int threat = 1;
        public int initiative = 1;
        public UnityEvent onAttack = new UnityEvent();
        public UnityEvent onReceiveHit = new UnityEvent();
        public UnityEvent onDeath = new UnityEvent();


        public int Hp { get; private set; }
        public int MaxHp { get; private set; }

        public int AttackPower { get; private set; }

        public int Threat { get; private set; }

        public int Initiative { get; private set; }

        public GameObject GameObject { get; private set; }
        public string Name { get { return gameObject.name; } }

        public UnityEvent OnAttack { get { return onAttack; } }
        public UnityEvent OnReceiveHit { get { return onReceiveHit; } }
        public UnityEvent OnDeath { get { return onDeath; } }

        public void Init()
        {
            Hp = hp;
            MaxHp = hp;
            AttackPower = attackPower;
            Threat = threat;
            Initiative = initiative;
            GameObject = gameObject;
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
            var damage = attacker.AttackPower;
            GameValueRegistry.Instance.SetRegistryValue("last_battle_hit_amount", damage.ToString());
            Hp -= damage;

            if (Hp < 0)
            {
                Hp = 0;
            }

            if (onReceiveHit != null)
            {
                onReceiveHit.Invoke();
            }

            if (!IsLiving())
            {
                onDeath.Invoke();
            }
        }

        public bool IsLiving()
        {
            return Hp > 0;
        }
    }
}