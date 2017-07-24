using RSG;
using UnityEngine;

namespace App.Simulation
{
    public interface ISimulationEvent
    {
        float Delay { get; }

        IPromise Start();
    }
}