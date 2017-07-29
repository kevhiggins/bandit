using System;
using System.Collections.Generic;
using System.Linq;
using App.Unit;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace App.Simulation.Cards
{
    [CreateAssetMenu(menuName = "Game/Cards/BaseCard")]
    public class BaseCard : Card
    {
        public List<Traveler> travelers = new List<Traveler>();
        public int travelerBaseline = 8;
        public int travelerVariance = 2;
        public float travelerSpawnInterval = 1;

        private TownManager townManager;

        [Inject]
        public void Construct(TownManager townManager)
        {
            this.townManager = townManager;
        }

        public override List<ISimulationEvent> GenerateEvents()
        {
            // Generate a random number between -n and n where n = variance
            var baselineModifier = Random.Range(-travelerVariance, travelerVariance);
            // Add the result to the baseline
            var travelerCount = travelerBaseline + baselineModifier;

            var events = new List<ISimulationEvent>();

            var startDelay = 0;
            // Generate the traveler events
            for (var i = 0; i < travelerCount; i++)
            {
                // Determine the start town.
                var startTown = townManager.GetRandomTown();

                // Determine the destination town.
                var endTown = townManager.GetDifferentTown(startTown);

                // Determine the delay.
                var delay = startDelay++;

                // Create the event and add it to the event list.
                var traveler = GetRandomTraveler();

                events.Add(new SpawnTravelerEvent(traveler, startTown, endTown, delay));
            }

            return events;
        }

        protected Traveler GetRandomTraveler()
        {
            if(!travelers.Any())
                throw new Exception("No travelers configured");

            var index = Random.Range(0, travelers.Count);
            return travelers.ElementAt(index);
        }

    }
}