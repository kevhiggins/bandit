using App.Unit;

namespace App.Simulation
{
    [System.Serializable]
    public class SpawnTravelerEvent : ISimulationEvent
    {
        public int Delay { get { return delay; } }
        public Traveler traveler;
        public int delay;
    }
}