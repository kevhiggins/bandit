using System;
using System.Collections.Generic;
using System.Linq;
using App.Extensions;
using App.Simulation.Cards;

namespace App.Simulation
{
    public class Deck
    {
        public List<Card> cards = new List<Card>();

        public ICard Draw()
        {
            var nextCard = cards.FirstOrDefault();
            if (nextCard == null)
            {
                throw new Exception("No card found in deck.");
            }
            // Move card to back of the deck.
            cards.Remove(nextCard);
            cards.Add(nextCard);
            return nextCard;
        }

        [Serializable]
        public class Settings
        {
            public List<Card.Settings> cards;

            public Deck GenerateDeck()
            {
                var deck = new Deck();
                foreach (var card in cards)
                {
                    for (var i = 0; i < card.frequency; i++)
                    {
                        deck.cards.Add(card.card);
                    }
                }
                deck.cards.Shuffle();

                return deck;
            }
        }
    }
}