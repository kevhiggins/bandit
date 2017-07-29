using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using App.Simulation.Cards;

namespace App.Simulation
{
    [Serializable]
    public class Deck
    {
        public List<Card> cards;

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
    }
}