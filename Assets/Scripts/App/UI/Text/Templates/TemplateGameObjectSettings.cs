using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.UI.Text.Templates
{
    [Serializable]
    public class TemplateGameObjectSettings
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
                var components = GetComponents();
                if (value < components.Length)
                {
                    componentName = components[value];
                }
            }
        }

        public string[] GetComponents()
        {
            var componentNames = new List<string>();

            if (gameObject == null)
                return componentNames.ToArray();

            Component[] components;
            /*
            var provider = GetProvider();

            
            if (provider != null)
            {
                var selected =   GetSelected(provider);
                if (selected == null)
                {
                    return componentNames.ToArray();
                }
                components = selected.GetComponents<Component>();
            }
            else
            {
            */
            components = gameObject.GetComponents<Component>();
            /*
            }
            */
            
            foreach (var component in components)
            {
                componentNames.Add(component.GetType().Name);
            }
            return componentNames.ToArray();
        }

        public Component GetComponent()
        {
            if (gameObject == null)
            {
                return null;
            }

            var provider = GetProvider();

            /*
            if (provider != null)
            {
                var selected = GetSelected(provider);
                if (selected == null)
                {
                    return null;
                }
                return selected.GetComponent(componentName);
            }
            */

            return gameObject.GetComponent(componentName);
        }

        protected ObjectProvider GetProvider()
        {
            return gameObject == null ? null : gameObject.GetComponent<ObjectProvider>();
        }

        /*
        public GameObject GetSelected(ObjectProvider provider)
        {
            if (provider.Selected == null)
                return null;

            var selectedGameObject = provider.Selected as GameObject;
            if (selectedGameObject != null)
            {
                return selectedGameObject;
            }
            var selectedObject = provider.Selected as Object;
            if (selectedObject != null)
            {
                
            }

            throw new Exception("Objects that are not of type GameObject not supported.");
        }
        */

        public string[] GetAttributes()
        {
            var component = GetComponent();
            Type type = component.GetType();
            var attributeNames = new List<string>();

            foreach (var f in type.GetFields().Where(f => f.IsPublic))
            {
                attributeNames.Add(f.Name);
            }

            foreach (var p in type.GetProperties().Where(p => p.CanRead && p.GetGetMethod(true).IsPublic))
            {
                attributeNames.Add(p.Name);
            }

            return attributeNames.ToArray();
        }
    }
}