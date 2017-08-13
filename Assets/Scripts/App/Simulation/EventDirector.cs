using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using App.Simulation;
using App.Simulation.Cards;
using App.UI.Events;
using C5;
using UnityEngine.Events;
using Zenject;

namespace App
{
    public class EventDirector : MonoBehaviour
    {
        public UnityEvent onSimulateStart = new UnityEvent();
        public UnityEvent onSimulateEnd = new UnityEvent();

        public GlobalEventManager globalEvents;

        public bool IsSimulating { get; private set; }

        private List<ISimulationEvent> activeEvents;
        private List<ISimulationEvent> completeEvents;
        private IntervalHeap<ISimulationEvent> eventQueue;
        private float simulationDuration = 0;

        private Deck eventDeck;

        [Inject]
        public void Construct(Deck eventDeck)
        {
            this.eventDeck = eventDeck;
        }

        void Start()
        {
            eventQueue = new IntervalHeap<ISimulationEvent>(new SimulationEventComparer());
        }

        protected void PopulateEventQueue()
        {
            var nextCard = eventDeck.Draw();
            eventQueue.AddAll(nextCard.GenerateEvents());
        }

        public void Simulate()
        {
            PopulateEventQueue();
            simulationDuration = 0;
            activeEvents = new List<ISimulationEvent>();
            completeEvents = new List<ISimulationEvent>();
            IsSimulating = true;
            onSimulateStart.Invoke();
        }

        void Update()
        {
            if (!IsSimulating)
                return;

            CheckSimulationComplete();

            if (!eventQueue.Any())
                return;

            simulationDuration += Time.deltaTime;

            var nextEvent = eventQueue.First();

            if (nextEvent.Delay <= simulationDuration)
            {
                var activeEvent = eventQueue.DeleteMin();
                activeEvents.Add(activeEvent);

                activeEvent.Start().Then(() =>
                {
                    activeEvents.Remove(activeEvent);
                    completeEvents.Add(activeEvent);
                    CheckSimulationComplete();
                });
            }
        }

        private void CheckSimulationComplete()
        {
            if (!eventQueue.Any() && !activeEvents.Any())
            {
                EndSimulation();
            }
        }

        private void EndSimulation()
        {
            IsSimulating = false;
            onSimulateEnd.Invoke();
        }
    }
}