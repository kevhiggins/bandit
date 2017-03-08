using UnityEngine;
using UnityEngine.UI;

namespace Bandit.UI
{
    class TextHook : MonoBehaviour
    {
        public string entryName;
        public string replacementText = "{value}";

        private string initialText;
        private Text textScript;

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
            textScript.text = initialText.Replace(replacementText, value);
        }

        // Register a list of categories and attributes that can be hooked into

        // Specify string to be replaced
        // Select the data that will be replacing the replacement string
    }
}
