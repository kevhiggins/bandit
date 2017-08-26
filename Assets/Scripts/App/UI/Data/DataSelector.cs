using System;
using System.Collections.Generic;
using System.Linq;
using App.ReactiveX;
using UnityEngine;

namespace App.UI.Data
{
    [Serializable]
    public class DataSelector
    {
        public virtual Type GenericType { get { return null; } }
        public object Selected
        {
            get
            {
                return SelectionPathIterator.Last();
            }
        }

        protected IEnumerable<object> SelectionPathIterator
        {
            get
            {
                object currentObject = GetTarget();
                yield return currentObject;
                foreach (var segment in pathSegments)
                {
                    currentObject = GetMemberValue(currentObject, segment);
                    yield return currentObject;

                    if (currentObject == null)
                    {
                        throw new Exception(string.Format("Object with type {0} and memberName {1} has a null member value", currentObject.GetType().FullName, segment));
                    }
                }
            }
        }

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

        private Action selectionCallback;

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

        public UnityEngine.Object GetTarget()
        {
            var target = source;
            var gameObject = SourceGameObject;

            if (gameObject == null) return target;

            if (selectedComponentName == null)
            {
                return null;
            }
            target = gameObject.GetComponent(selectedComponentName);
            return target;
        }

        protected void WatchObservables()
        {
            foreach(var currentObject in SelectionPathIterator)
            {
                var observable = currentObject as IObservable;
                if (observable != null)
                {
                    observable.Subscribe(test =>
                    {
                        Debug.Log("HI");
                    });
                }
            }
        }

        public void RegisterSelectionCallback(Action callback)
        {
            selectionCallback = callback;
            callback();
        }

        protected Type GetMemberType(Type objectType, string memberName)
        {
            var field = objectType.GetField(memberName);
            if (field != null)
            {
                return field.FieldType;
            }

            var property = objectType.GetProperty(memberName);
            if (property != null)
            {
                return property.PropertyType;
            }
            throw new Exception(string.Format("Object type {0} with member name {1} does not have a corresponding type.", objectType.FullName, memberName));
        }

        protected object GetMemberValue(object obj, string memberName)
        {
            var objType = obj.GetType();
            var field = objType.GetField(memberName);
            object value = null;
            if (field != null)
            {
                value = field.GetValue(obj);
            }

            var property = objType.GetProperty(memberName);
            if (property != null)
            {
                value = property.GetValue(obj, null);
            }

            return value;
        }

        public Type GetTypeAtPath(List<string> path)
        {
            // If source is a game object, get the selected component.
            var target = GetTarget();
            if (target == null)
            {
                return null;
            }

            var currentType = target.GetType();

            foreach (var pathSegment in path)
            {
                currentType = GetMemberType(currentType, pathSegment);
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