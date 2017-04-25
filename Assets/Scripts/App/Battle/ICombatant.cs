using UnityEngine;
using UnityEngine.Events;

namespace App.Battle
{
    public interface ICombatant
    {
        int Hp { get; }
        int MaxHp { get; }
        int AttackPower { get; }
        int Threat { get; }
        int Initiative { get; }
        GameObject GameObject { get; }
        string Name { get; }

        UnityEvent OnAttack { get; }
        UnityEvent OnReceiveHit { get; }
        UnityEvent OnDeath { get; }



        void Attack(ICombatant target);
        void ReceiveHit(ICombatant attacker);

        bool IsLiving();
    }
}
