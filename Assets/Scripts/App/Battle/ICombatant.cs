using UnityEngine;
using UnityEngine.Events;

namespace App.Battle
{
    public interface ICombatant
    {
        int Hp { get; }
        int AttackPower { get; }
        int Threat { get; }
        int Initiative { get; }
        GameObject GameObject { get; }
        string Name { get; }

        UnityEvent OnAttack { get; }
        UnityEvent OnReceiveHit { get; }



        void Attack(ICombatant target);
        void ReceiveHit(ICombatant attacker);

        bool IsLiving();
    }
}
