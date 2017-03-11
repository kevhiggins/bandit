using UnityEngine;

namespace App.Unit
{
    public class Unit : MonoBehaviour, IUnit
    {
        public void Despawn()
        {
            Destroy(gameObject);
        }
    }
}
