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

        [NonSerialized]
        private bool isDeckInjected = false;

        public override void InstallBindings()
        {
            // TODO Cards should be CardSetting objects containing a quantity.
            // TODO We should generate the correct number of cards from those settings.
            // TODO figure out if there is a way to match the card setting to the appropriate service, so we only have to inject the card event resolvers instead of each card.

            // Bind Deck, and inject into the deck and all of its cards on the first run.
            Container.Bind<Deck>().FromMethod(ctx =>
            {
                Debug.Log(isDeckInjected);
                if (isDeckInjected)
                    return eventDeck;

                isDeckInjected = true;

                Container.Inject(eventDeck);

                foreach (var card in eventDeck.cards)
                {
                    Container.Inject(card);
                }

                Container.Bind<Deck>().FromInstance(eventDeck);

                return eventDeck;
            });
        }
    }
}