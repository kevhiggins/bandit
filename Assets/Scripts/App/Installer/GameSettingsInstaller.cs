using System;
using App.Simulation;
using UnityEngine;
using Zenject;

namespace App.Installer
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public Deck.Settings eventDeck;

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
    }
}