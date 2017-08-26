using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace App.Editor
{
    public class BasePropertyDrawer : PropertyDrawer
    {
        public object GetParent(SerializedProperty prop)
        {
            var elements = GetPropertyPathSegments(prop);
            object obj = prop.serializedObject.targetObject;
            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        public int GetNestingLevel(SerializedProperty prop)
        {
            var segments = GetPropertyPathSegments(prop);
            return segments.Length - 1;
        }

        protected string[] GetPropertyPathSegments(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            return path.Split('.');
        }

        public object GetValue(object source, string name)
        {
            if (source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        public object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            var isValid = false;
            while (index-- >= 0)
                isValid = enm.MoveNext();
            return isValid ? enm.Current : null;
        }
    }
}