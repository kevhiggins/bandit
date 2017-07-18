using UnityEngine;

namespace App.Location
{
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class AmbushLocation : AbstractLocation
    {
        void Start()
        {
            Behaviour halo = (Behaviour)GetComponent("Halo");
            halo.enabled = false;
        }
    }
}