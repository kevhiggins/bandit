using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.UI.Text.Templates
{
    [Serializable]
    public class TemplateObjectSettings
    {
        public string key;

        public GameObject gameObject;

        [SerializeField]
        [HideInInspector]
        private string componentName;

        public int ComponentIndex
        {
            get
            {
                var i = 0;
                foreach (var component in GetComponents())
                {
                    if (component == componentName)
                    {
                        return i;
                    }
                    i++;
                }
                return 0;
            }
            set
            {
                componentName = GetComponents()[value];
            }
        }

        public string[] GetComponents()
        {
            var componentNames = new List<string>();
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                componentNames.Add(component.GetType().Name);
            }
            return componentNames.ToArray();
        }

        public Component GetComponent()
        {
            return gameObject.GetComponent(componentName);
        }

        public string[] GetAttributes()
        {
            var component = GetComponent();
            Type type = component.GetType();
            var attributeNames = new List<string>();

            foreach (var f in type.GetFields().Where(f => f.IsPublic))
            {
                attributeNames.Add(f.Name);
                //Console.WriteLine(
                    //String.Format("Name: {0} Value: {1}", f.Name, f.GetValue(obj));
            }
            return attributeNames.ToArray();
        }
    }
}