using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using App.Installer;
using App.Simulation.Cards;

[CustomEditor(typeof(DeckSettingsInstaller), true)]
public class DeckEditor : Editor
{
    public DeckSettingsInstaller DeckSettingsInstaller;

    void OnEnable()
    {
        this.DeckSettingsInstaller = (DeckSettingsInstaller) target;
    }

    public override void OnInspectorGUI()
    {
        var deckSettings = DeckSettingsInstaller.eventDeck;

        serializedObject.Update();


        GUILayout.Label("Deck", EditorStyles.boldLabel);

        var deckCount = deckSettings.cards.Sum((cardSetting) => cardSetting.frequency);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Total Cards: " + deckCount, GUILayout.ExpandWidth(false));
        GUILayout.EndHorizontal();

        var cardSettingsToDelete = new List<Card.Settings>();

        foreach (var cardSetting in deckSettings.cards)
        {
            
            GUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 75;
            cardSetting.frequency = EditorGUILayout.IntField("Quantity", cardSetting.frequency, GUILayout.MaxWidth(125));
            EditorGUIUtility.labelWidth = 50;
            cardSetting.card = (Card)EditorGUILayout.ObjectField("Card", cardSetting.card, typeof(Card), false, GUILayout.MaxWidth(200));
            EditorGUIUtility.labelWidth = 0;

            var cardDrawProbability = cardSetting.frequency / (float)deckCount * 100;
            GUILayout.Label(cardDrawProbability.ToString("00.00") + "%");

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                cardSettingsToDelete.Add(cardSetting);
            }

            GUILayout.EndHorizontal();
        }

        foreach (var cardSettingToDelete in cardSettingsToDelete)
        {
            deckSettings.cards.Remove(cardSettingToDelete);
        }

        if (GUILayout.Button("Add", GUILayout.Width(75)))
        {
            deckSettings.cards.Add(new Card.Settings());
        }

        EditorUtility.SetDirty(DeckSettingsInstaller);

    }
}