using System;
using System.Collections.Generic;
using System.Linq;
using App.UI.Text.Templates;
using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomPropertyDrawer(typeof(TemplateObjectSettings))]
    public class TemplateObjectSettingsDrawer : PropertyDrawer
    { 
        private TemplateObjectSettings templateObjectSettings;

        protected TemplateObjectSettings GetTarget(SerializedProperty property)
        {
            var value = fieldInfo.GetValue(property.serializedObject.targetObject);
            if (value is TemplateObjectSettings)
            {
                return (TemplateObjectSettings)value;
            }

            if (!(value is List<TemplateObjectSettings>))
            {
                return null;
            }

            var obj = ((List<TemplateObjectSettings>)fieldInfo.GetValue(property.serializedObject.targetObject)).ToArray();
            TemplateObjectSettings target = null;
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

            var labelPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 2;

            var keyRect = new Rect(position.x, position.y + 18, 500, 16);
            var gameObjectRect = new Rect(position.x, position.y + 36, 500, 16);
            var componentsRect = new Rect(position.x, position.y + 54, 500, 16);
            var attributesRect = new Rect(position.x, position.y + 72, 500, 16);

            
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"));
            EditorGUI.PropertyField(gameObjectRect, property.FindPropertyRelative("gameObject"));

            target.ComponentIndex = EditorGUI.Popup(componentsRect, "Component", target.ComponentIndex, target.GetComponents());
            var selectedComponent = target.GetComponent();
            if (selectedComponent != null)
            {
                EditorGUI.Popup(attributesRect, "Attributes", 0, target.GetAttributes());
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 100;
        }
    }
}