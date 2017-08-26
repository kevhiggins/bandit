using System;
using System.Collections.Generic;
using System.Linq;
using App.UI.Data;
using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomPropertyDrawer(typeof(DataSelector), true)]
    public class DataSelectorDrawer : BasePropertyDrawer
    {
        protected DataSelector GetTarget(SerializedProperty property)
        {
            return GetParent(property) as DataSelector;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var target = GetTarget(property);
            if (target == null)
                return;

            var level = GetNestingLevel(property);

            EditorGUI.BeginProperty(position, label, property);

            RenderBackgroundColor(target, position, property, label);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            var indentPerNestedListLevel = 2;
            EditorGUI.indentLevel = 1 + level * indentPerNestedListLevel;

            RenderSelector(position, property, target);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        private void RenderBackgroundColor(DataSelector target, Rect position, SerializedProperty property, GUIContent label)
        {
            if (target.GenericType == null || target.SelectedType == null)
            {
                return;
            }

            Color backgroundColor;
            backgroundColor = target.GenericType == target.SelectedType ? Color.green : Color.red;

            var texture = MakeTex(500, (int)GetPropertyHeight(property, label), backgroundColor);
            var rect = new Rect(position.x, position.y, texture.width, texture.height);

            var style = new GUIStyle();
            style.normal.background = texture;
            EditorGUI.LabelField(rect, GUIContent.none, style);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        protected void FillBackgroundTexture(Texture2D texture)
        {
            
            var fillColor = Color.green;
            var fillColorArray = texture.GetPixels();

            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = fillColor;
            }

            texture.SetPixels(fillColorArray);

            texture.Apply();
        }

        protected void RenderSelector(Rect position, SerializedProperty property, DataSelector target)
        {
            var sourceRect = new Rect(position.x, position.y + 18, 500, 16);
            var componentsRect = new Rect(position.x, position.y + 36, 500, 16);
            var attributesRect = new Rect(position.x, position.y + 54, 500, 16);

            EditorGUI.PropertyField(sourceRect, property.FindPropertyRelative("source"));

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