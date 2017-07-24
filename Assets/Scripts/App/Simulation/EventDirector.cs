using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using App.Simulation;
using C5;
using UnityEngine.Events;

namespace App
{
    public class EventDirector : MonoBehaviour
    {
        public List<SpawnTravelerEvent> spawnTravelerEvents;
        private IntervalHeap<ISimulationEvent> eventQueue;
        private float simulationDuration = 0;
        public bool IsSimulating { get; private set; }
        private List<ISimulationEvent> activeEvents;
        private List<ISimulationEvent> completeEvents;

        public UnityEvent onSimulateStart = new UnityEvent();
        public UnityEvent onSimulateEnd = new UnityEvent();

        void Start()
        {
            eventQueue = new IntervalHeap<ISimulationEvent>(new SimulationEventComparer());
        }

        protected void PopulateEventQueue()
        {
            foreach (var spawnTravelerEvent in spawnTravelerEvents)
            {
                if (!eventQueue.Add(spawnTravelerEvent))
                {
                    throw new Exception("Failed to add event.");
                }
            }
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
            if (!IsSimulating || !eventQueue.Any())
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