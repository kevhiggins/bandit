using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using App.Simulation;
using App.Simulation.Cards;
using App.UI.Events;
using C5;
using UniRx;
using UnityEngine.Events;
using Zenject;

namespace App
{
    public class EventDirector : MonoBehaviour
    {
        public UnityEvent onSimulateStart = new UnityEvent();
        public UnityEvent onSimulateEnd = new UnityEvent();

        public GlobalEventManager globalEvents;

        public ReadOnlyReactiveProperty<bool> IsSimulating;
        private ReactiveProperty<bool> isSimulating;

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

        void Awake()
        {
            isSimulating = new ReactiveProperty<bool>(false);
            IsSimulating = new ReadOnlyReactiveProperty<bool>(isSimulating);
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
            isSimulating.Value = true;
            onSimulateStart.Invoke();
        }

        void Update()
        {
            if (!IsSimulating.Value)
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
            isSimulating.Value = false;
            onSimulateEnd.Invoke();
        }
    }
}