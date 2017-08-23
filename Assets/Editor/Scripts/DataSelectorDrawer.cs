﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.UI.Data;
using App.UI.Text.Templates;
using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomPropertyDrawer(typeof(DataSelector))]
    public class DataSelectorDrawer : PropertyDrawer
    {
        protected DataSelector GetTarget(SerializedProperty property)
        {
            var value = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (value is DataSelector)
            {
                return (DataSelector)value;
            }

            if (!(value is List<DataSelector>))
            {
                return null;
            }

            var obj = ((List<DataSelector>)fieldInfo.GetValue(property.serializedObject.targetObject)).ToArray();
            DataSelector target = null;
            if (obj.GetType().IsArray)
            {
                var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));

                if (index >= obj.Length)
                {
                    return null;
                }
                target = obj[index];
            }
            return target;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = GetTarget(property);
            if (target == null)
                return;

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            RenderSelector(position, property, target);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        protected void RenderSelector(Rect position, SerializedProperty property, DataSelector target)
        {
            var sourceRect = new Rect(position.x, position.y + 18, 500, 16);
            var componentsRect = new Rect(position.x, position.y + 36, 500, 16);
            var attributesRect = new Rect(position.x, position.y + 54, 500, 16);


            //EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"));
            EditorGUI.PropertyField(sourceRect, property.FindPropertyRelative("source"));

            //target.source = EditorGUI.ObjectField(sourceRect, "Source", target.source, typeof(object), true);

            if (target.source == null)
            {
                return;
            }

            // Component dropdown.
            var componentNames = target.GetComponentNames();
            if (componentNames != null)
            {
                var componentIndex = target.ComponentIndex;
                componentNames.Insert(0, "No Selection");
                target.ComponentIndex = EditorGUI.Popup(componentsRect, "Component", target.ComponentIndex, componentNames.ToArray());

                // If the selected component changes, then reset the path segment list.
                if (componentIndex != target.ComponentIndex)
                {
                    target.PathSegments = new List<string>();
                }
            }

            // If we have a component dropdown, but haven't made a selection, then skip rendering the attribute dropdowns.
            if (componentNames != null && target.ComponentIndex == 0)
            {
                return;
            }


            var segments = new List<string>();
            using (var enumerator = target.PathSegments.GetEnumerator())
            {
                do
                {
                    string current;
                    if (!enumerator.MoveNext())
                    {
                        current = null;
                    }
                    else
                    {
                        current = enumerator.Current;
                    }

                    var selectedSegment = RenderAttributes(target, attributesRect, segments, current);
                    if (selectedSegment == null)
                    {
                        break;
                    }

                    segments.Add(selectedSegment);
                    attributesRect.y += 18;
                } while (true);
            }
            target.PathSegments = segments;
        }

        protected string RenderAttributes(DataSelector target, Rect rect, List<string> segments, string value)
        {
            var attributes = target.GetAttributes(segments);

            attributes.Insert(0, "No Selection");
            var index = value == null ? 0 : attributes.FindIndex(a => a == value);

            var newIndex = EditorGUI.Popup(rect, index, attributes.ToArray());

            return newIndex <= 0 ? null : attributes[newIndex];
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var target = GetTarget(property);
            if (target == null)
                return 100;

            return 100 + target.PathSegments.Count * 18;
        }
    }
}