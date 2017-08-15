using System;
using TMPro;
using UnityEngine;

namespace App.UI.Text
{
    public class TextAdapter : IText
    {
        private UnityEngine.UI.Text text;
        private TextMeshProUGUI textMeshPro;


        public TextAdapter(GameObject gameObject)
        {
            text = gameObject.GetComponent<UnityEngine.UI.Text>();
            if (text != null)
            {
                return;
            }

            textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                NotFound();
            }
        }

        public string Value
        {
            get
            {
                if (text != null)
                {
                    return text.text;
                }
                return textMeshPro.text;
            }

            set
            {
                if (text != null)
                {
                    text.text = value;
                }
                else
                {
                    textMeshPro.text = value;
                }
            }
        }

        protected void NotFound()
        {
            throw new Exception("No valid UnityEngine.UI.Text or TextMeshProUGUI components exist on the provided game object.");
        }
    }
}