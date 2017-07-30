using System;
using App.Simulation;
using UnityEngine;
using Zenject;

namespace App.Installer
{
    [CreateAssetMenu(menuName = "Game/Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        public Deck eventDeck;

        public override void InstallBindings()
        {
            // Bind Deck, and inject into the deck and all of its cards.
            Container.Bind<Deck>().FromMethod(ctx =>
            {
                Container.Inject(eventDeck);

                foreach (var card in eventDeck.cards)
                {
                    Container.Inject(card);
                }

                return eventDeck;
            }).AsSingle();
        }
    }
}