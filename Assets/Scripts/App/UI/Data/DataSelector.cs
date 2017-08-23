using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Extensions;
using App.UI.Text.Templates;
using ModestTree;
using UnityEngine;

namespace App.UI.Data
{
    [Serializable]
    public class DataSelector
    {
        public UnityEngine.Object source;
        public List<string> PathSegments {
            get { return pathSegments;  }
            set { pathSegments = value; }
        }

        protected GameObject SourceGameObject {
            get
            {
                var gameObject = source as GameObject;

                if (gameObject != null) return gameObject;
                var component = source as Component;
                return component != null ? component.gameObject : null;
            }
        }

        [SerializeField]
        [HideInInspector]
        private string selectedComponentName;

        [SerializeField]
        [HideInInspector]
        private List<string> pathSegments;

        public int ComponentIndex
        {
            get
            {
                if (selectedComponentName == null)
                    return 0;

                var componentNames = GetComponentNames();
                return componentNames.FindIndex(c => c == selectedComponentName) + 1;
            }
            set
            {
                var componentNames = GetComponentNames();
                selectedComponentName = value == 0 ? null : componentNames[value - 1];
            }
        }

        public List<string> GetComponentNames()
        {
            if (source == null)
                return null;

            var gameObject = SourceGameObject;

            if (gameObject == null)
                return null;

            var components = gameObject.GetComponents<Component>();

            return components.Select(component => component.GetType().Name).ToList();
        }

        public Component GetComponent()
        {
            var gameObject = SourceGameObject;

            if (gameObject == null || selectedComponentName == null)
            {
                return null;
            }

            return gameObject.GetComponent(selectedComponentName);
        }

        public List<string> GetAttributes(List<string> pathSegments)
        {
            // If source is a game object, get the selected component.
            var gameObject = SourceGameObject;
            if (gameObject != null)
            {
                if (selectedComponentName == null)
                {
                    return null;
                }
                source = gameObject.GetComponent(selectedComponentName);
            }
            
            Type currentType = source.GetType();

            foreach (var pathSegment in pathSegments)
            {
                var field = currentType.GetField(pathSegment);
                if (field != null)
                {
                    currentType = field.FieldType;
                    continue;
                }

                var property = currentType.GetProperty(pathSegment);
                if (property != null)
                {
                    currentType = property.PropertyType;
                    continue;
                }

                throw new Exception(string.Format("Path segment with name {0} does not have a corresponding value.", pathSegment));              
            }

            // Get list of fields and properties on current object.
            var attributeNames = currentType.GetFields().Where(f =>
            {
                if (!f.IsPublic) return false;

                var attributes = f.GetCustomAttributes(true);
                return !attributes.OfType<ObsoleteAttribute>().Any();
            }).Select(f => f.Name).ToList();

            attributeNames.AddRange(currentType.GetProperties().Where(p =>
                {
                    if (!p.CanRead || !p.GetGetMethod(true).IsPublic) return false;

                    var attributes = p.GetCustomAttributes(true);
                    return !attributes.OfType<ObsoleteAttribute>().Any();
                }).Select(p => p.Name));

            return attributeNames;
        }
    }
}