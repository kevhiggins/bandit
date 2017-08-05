using System;
using System.Collections.Generic;
using Antlr4.StringTemplate;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Text.Templates
{
    public class TemplateInjector : MonoBehaviour
    {
        [TextArea(5, 10)]
        public string template;

        public List<TemplateObjectSettings> objects = new List<TemplateObjectSettings>();

        // TODO allow support for UniRx ReactiveProperty

        private UnityEngine.UI.Text text;
        private Template templateRenderer;

        void Awake()
        {
            text = GetComponent<UnityEngine.UI.Text>();
            if (text == null)
            {
                throw new Exception("Failed to find Text component.");
            }
            templateRenderer = new Template(template);

            foreach (var templateObject in objects)
            {
                var component = templateObject.GetComponent();
                if (string.IsNullOrEmpty(templateObject.key))
                {
                    continue;
                }

                templateRenderer.Add(templateObject.key, component);
            }

            Render();
        }

        protected void Render()
        {
            text.text = templateRenderer.Render();
        }
    }
}