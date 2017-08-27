using System.Collections.Generic;
using Antlr4.StringTemplate;
using UnityEngine;

namespace App.UI.Text.Templates
{
    public class TemplateInjector : MonoBehaviour
    {
        [TextArea(5, 10)]
        public string template;

        public char delimiterStartChar = '<';
        public char delimiterStopChar = '>';

        public List<TemplateObjectSettings> objects = new List<TemplateObjectSettings>();

        private IText text;
        private Template templateRenderer;

        void Awake()
        {
            text = new TextAdapter(gameObject);
            templateRenderer = new Template(template, delimiterStartChar, delimiterStopChar);

            foreach (var objectSetting in objects)
            {
                if (string.IsNullOrEmpty(objectSetting.key))
                {
                    continue;
                }

                var setting = objectSetting;
                objectSetting.sourceObject.RegisterSelectionCallback(() =>
                {
                    var attributes = templateRenderer.GetAttributes();
                    var attributeExists = attributes != null && attributes.ContainsKey(setting.key);
                    if (attributeExists)
                    {
                        templateRenderer.Remove(setting.key);
                    }
                    templateRenderer.Add(setting.key, setting.sourceObject.Selected);

                    // If we just changed an existing attribute, then re-render the template.
                    if (attributeExists)
                    {
                        Render();
                    }
                });
            }
            Render();
        }

        protected void Render()
        {
            text.Value = templateRenderer.Render();
        }
    }
}