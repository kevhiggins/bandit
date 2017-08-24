using App.UI.Data;
using UnityEngine;

namespace App.UI.Sprite
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