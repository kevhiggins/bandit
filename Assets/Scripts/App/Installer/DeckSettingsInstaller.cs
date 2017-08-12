using System;
using App.Simulation;
using App.UI.Location;
using UnityEngine;
using Zenject;

namespace App.Installer
{
    [CreateAssetMenu(menuName = "Game/Deck Settings")]
    public class DeckSettingsInstaller : ScriptableObjectInstaller<DeckSettingsInstaller>
    {
        public Deck.Settings eventDeck;
        public PrefabSettings prefabSettings;

        public override void InstallBindings()
        {
            // Bind Deck, and inject into the deck and all of its cards.
            Container.Bind<Deck>().FromMethod(ctx =>
            {
                var deck = eventDeck.GenerateDeck();

                Container.Inject(deck);

                foreach (var card in deck.cards)
                {
                    Container.Inject(card);
                }

                return deck;
            }).AsSingle();
        }

        public class PrefabSettings
        {
            public LocationJobIcons locationJobIcons;
        }
    }
}