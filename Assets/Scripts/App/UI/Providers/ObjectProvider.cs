using App.ReactiveX;
using UnityEngine;

namespace App.UI.Providers
{
    public class ObjectProvider<T> : ScriptableObject
    {
        public ReactiveProperty<T> Selected = new ReactiveProperty<T>();
    }
}