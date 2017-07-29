using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Simulation.Cards
{
    [Serializable]
    public abstract class Card : ScriptableObject, ICard
    {
        public abstract List<ISimulationEvent> GenerateEvents();
    }
}