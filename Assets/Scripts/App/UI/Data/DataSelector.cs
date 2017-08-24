using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace App.UI.Data
{
    [Serializable]
    public class DataSelector
    {
        public virtual Type GenericType { get { return null; } }
        public UnityEngine.Object source;
        public List<string> PathSegments {
            get { return pathSegments;  }
            set { pathSegments = value; }
        }

        public Type SelectedType {
            get { return GetTypeAtPath(pathSegments); }
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
        private List<string> pathSegments = new List<string>();

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

        public Type GetTypeAtPath(List<string> path)
        {
            // If source is a game object, get the selected component.
            var target = source;
            var gameObject = SourceGameObject;
            if (gameObject != null)
            {
                if (selectedComponentName == null)
                {
                    return null;
                }
                target = gameObject.GetComponent(selectedComponentName);
            }

            var currentType = target.GetType();

            foreach (var pathSegment in path)
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

                throw new Exception(string.Format("Path segment with name {0} does not have a corresponding type.", pathSegment));
            }

            return currentType;
        }

        public List<string> GetAttributes(List<string> path)
        {
            var currentType = GetTypeAtPath(path);

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