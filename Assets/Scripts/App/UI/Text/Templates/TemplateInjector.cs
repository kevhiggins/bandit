using System;
using System.Collections.Generic;
using System.ComponentModel;
using Antlr4.StringTemplate;
using App.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace App.UI.Text.Templates
{
    public class TemplateInjector : MonoBehaviour
    {
        [TextArea(5, 10)]
        public string template;

        public List<TemplateGameObjectSettings> gameObjects = new List<TemplateGameObjectSettings>();
        public List<TemplateObjectSettings> objects = new List<TemplateObjectSettings>();

        // TODO allow support for UniRx ReactiveProperty

        private IText text;
        private Template templateRenderer;

        void Awake()
        {
            text = new TextAdapter(gameObject);
            templateRenderer = new Template(template);

            foreach (var templateGameObject in gameObjects)
            {
                var component = templateGameObject.GetComponent();
                if (string.IsNullOrEmpty(templateGameObject.key))
                {
                    continue;
                }

                var objectProvider = component as ObjectProvider;
                if (objectProvider != null)
                {
                    var o = templateGameObject;
                    templateRenderer.Add(templateGameObject.key, objectProvider.Selected);
                    objectProvider.PropertyChanged += (sender, args) =>
                    {
                        templateRenderer.Remove(o.key);
                        templateRenderer.Add(o.key, objectProvider.Selected);
                        Render();
                    };
                }
                else
                {
                    templateRenderer.Add(templateGameObject.key, component);
                }
            }

            foreach (var templateObject in objects)
            {
                templateRenderer.Add(templateObject.key, templateObject.sourceObject);
            }

            Render();
        }

        protected void Render()
        {
            text.Value = templateRenderer.Render();
        }
    }
}