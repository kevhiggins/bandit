using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI
{
    public class TextHook : AppMonoBehavior
    {
        [HideInInspector]
        public string entryName;

        public string replacementText = "{value}";

        public bool synchronizeText = true;

        private string initialText;
        private string initialProText;
        private Text textScript;
        private TextMeshProUGUI textMeshProScript;

        private string currentValue;

        protected override void Awake()
        {
            textMeshProScript = gameObject.GetComponent<TextMeshProUGUI>();
            textScript = gameObject.GetComponent<Text>();
            if (textScript != null)
            {
                initialText = textScript.text;
            }
            if (textMeshProScript != null)
            {
                initialProText = textMeshProScript.text;
            }
            

            base.Awake();
        }

        protected override void Init()
        {
            if (synchronizeText)
            {
                GameValueRegistry.Instance.RegisterHandler(entryName, ReplaceText);
            }

            var value = GameValueRegistry.Instance.GetValue(entryName);
            ReplaceText(value);
        }

        private void ReplaceText(string value)
        {
            currentValue = value;

            if (textMeshProScript != null)
            {
                textMeshProScript.text = initialProText.Replace(replacementText, value);
            }
            if (textScript != null)
            {
                textScript.text = initialText.Replace(replacementText, value);
            }

            var textHooks = GetComponents<TextHook>();
            foreach (var hook in textHooks)
            {
                if (hook != this)
                {
                    hook.SyncText();
                }
            }
        }

        /**
         * Called when another TextHook script replaces its text.
         */
        private void SyncText()
        {
            if (textScript != null)
            {
                textScript.text = textScript.text.Replace(replacementText, currentValue);
            }
            if (textMeshProScript != null)
            {
                textMeshProScript.text = textMeshProScript.text.Replace(replacementText, currentValue);
            }
        }
    }
}
