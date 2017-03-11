using System.Collections.Generic;
using System.Linq;
using App.UI;
using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    [CustomEditor(typeof(TextHook), true)]
    public class TextHookEditor : UnityEditor.Editor
    {
        private string[] categories;

        private int categoryIndex;
        private int previousCategoryIndex;
        private int valueIndex;

        private Dictionary<string, CategoryData> categoryData;

        private TextHook targetHook;

        void OnEnable()
        {
            targetHook = (TextHook)target;

            categoryData = new Dictionary<string, CategoryData>();

            var categorySet = new HashSet<string>();


            var entryIndex = 0;
            foreach (KeyValuePair<string, Dictionary<string, string>> entry in GameValueRegistry.registry)
            {
                var attributeNames = new List<string>();
                var attributeTitles = new List<string>();
                categorySet.Add(entry.Key);

                var innerIndex = 0;
                foreach (KeyValuePair<string, string> innerEntry in entry.Value)
                {
                    if (innerEntry.Key == targetHook.entryName)
                    {
                        valueIndex = innerIndex;
                        categoryIndex = entryIndex;
                    }

                    attributeNames.Add(innerEntry.Key);
                    attributeTitles.Add(innerEntry.Value);
                    innerIndex++;
                }

                categoryData.Add(entry.Key, new CategoryData(attributeNames.ToArray(), attributeTitles.ToArray()));
                entryIndex++;
            }

            categories = categorySet.ToArray();
            previousCategoryIndex = categoryIndex;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDefaultInspector();

            categoryIndex = EditorGUILayout.Popup("Category", categoryIndex, categories);

            // Reset value index if the category has changed.
            if (categoryIndex != previousCategoryIndex)
            {
                valueIndex = 0;
            }

            var currentCategory = categories[categoryIndex];

            var titles = categoryData[currentCategory].Titles;

            valueIndex = EditorGUILayout.Popup("Value", valueIndex, titles);

            targetHook.entryName = categoryData[currentCategory].Names[valueIndex];
            previousCategoryIndex = categoryIndex;
        }
    }
}
