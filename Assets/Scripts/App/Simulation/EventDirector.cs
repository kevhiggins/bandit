using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using App.Simulation;
using C5;

namespace App
{
    public class EventDirector : MonoBehaviour
    {
        public List<SpawnTravelerEvent> spawnTravelerEvents;
        private IntervalHeap<ISimulationEvent> eventQueue;

        void Start()
        {
            eventQueue = new IntervalHeap<ISimulationEvent>(new SimulationEventComparer());

            foreach (var spawnTravelerEvent in spawnTravelerEvents)
            {
                if (!eventQueue.Add(spawnTravelerEvent))
                {
                   throw new Exception("Failed to add event."); 
                }
            }
        }
    }
}