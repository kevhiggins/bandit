using App.ReactiveX;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.UI.Text.Templates
{
    public class ObjectProvider : MonoBehaviour
    {
        public ReactiveProperty<Object> Selected = new ReactiveProperty<Object>();
    }
}