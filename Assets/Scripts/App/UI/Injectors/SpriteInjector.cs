using App.UI.Data;
using UnityEngine;

namespace App.UI.Injectors
{
    public class SpriteInjector : MonoBehaviour
    {
        public SpriteDataSelector selector;

        void Awake()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = selector.GenericSelected;
        }
    }
}