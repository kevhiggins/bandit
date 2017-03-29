using UnityEngine;

namespace App.Battle
{
    public interface ICombatant
    {
        int Hp { get; }
        int AttackPower { get; }
        int Threat { get; }
        GameObject DisplayPrefab { get; }

        void Attack(ICombatant target);
    }
}
