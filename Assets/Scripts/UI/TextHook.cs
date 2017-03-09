﻿using UnityEngine;
using UnityEngine.UI;

namespace Bandit.UI
{
    public class TextHook : MonoBehaviour
    {
        [HideInInspector]
        public string entryName;

        public string replacementText = "{value}";

        private string initialText;
        private Text textScript;

        private string currentValue;

        void Awake()
        {
            textScript = gameObject.GetComponent<Text>();
            initialText = textScript.text;

            GameManager.OnAfterInit += () =>
            {
                GameManager.instance.GameValueRegistry.RegisterHandler(entryName, ReplaceText);
                var value = GameManager.instance.GameValueRegistry.GetValue(entryName);
                ReplaceText(value);
            };
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