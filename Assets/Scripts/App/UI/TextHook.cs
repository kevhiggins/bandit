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
        private Text textScript;

        private string currentValue;

        protected override void Awake()
        {
            textScript = gameObject.GetComponent<Text>();
            initialText = textScript.text;
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
            textScript.text = initialText.Replace(replacementText, value);

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
            textScript.text = textScript.text.Replace(replacementText, currentValue);
        }
    }
}
