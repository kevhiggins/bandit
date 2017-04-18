using UnityEngine;

namespace App.Battle
{
    public interface ICombatant
    {
        int Hp { get; }
        int AttackPower { get; }
        int Threat { get; }
        int Initiative { get; }
        GameObject DisplayPrefab { get; }

        void Attack(ICombatant target);
        void ReceiveHit(ICombatant attacker);

        bool IsLiving();
    }
}
