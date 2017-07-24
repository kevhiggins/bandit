using RSG;

namespace App.Simulation
{
    public abstract class AbstractSimulationEvent : ISimulationEvent
    {
        public float Delay { get; private set; }
        public IPromise Start()
        {
            throw new System.NotImplementedException();
        }

        protected AbstractSimulationEvent(int delay)
        {
            this.Delay = delay;
        }
    }
}