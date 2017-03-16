using UnityEngine;

namespace App.Unit
{
    public abstract class AbstractUnit : MonoBehaviour, IUnit
    {
        public void Despawn()
        {
            Destroy(gameObject);
        }
    }
}
