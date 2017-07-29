using System.Collections.Generic;

namespace App.Simulation.Cards
{
    public interface ICard
    {
        List<ISimulationEvent> GenerateEvents();
    }
}