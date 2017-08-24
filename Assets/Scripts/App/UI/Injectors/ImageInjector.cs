using App.UI.Data;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Injectors
{
    public class ImageInjector : MonoBehaviour
    {
        public SpriteDataSelector selector;

        void Awake()
        {
            var image = GetComponent<Image>();
            image.sprite = selector.GenericSelected;
        }
    }
}