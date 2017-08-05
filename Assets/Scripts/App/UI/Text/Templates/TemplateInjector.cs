using System;
using Antlr4.StringTemplate;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Text.Templates
{
    public class TemplateInjector : MonoBehaviour
    {
        [TextArea(5, 10)]
        public string template;

        // TODO allow support for UniRx ReactiveProperty

        private UnityEngine.UI.Text text;

        void Awake()
        {
            text = GetComponent<UnityEngine.UI.Text>();
            if (text == null)
            {
                throw new Exception("Failed to find Text component.");
            }
            Render();
        }

        // Render
        // Slots for configuring data sources should be observable, so when we swap sources the template is rerendered

        protected void Render()
        {
            var templateRenderer = new Template(template);
            templateRenderer.Add("name", "BOB");
            text.text = templateRenderer.Render();
        }
    }
}