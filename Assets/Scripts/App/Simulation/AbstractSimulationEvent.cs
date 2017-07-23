namespace App.Simulation
{
    public abstract class AbstractSimulationEvent : ISimulationEvent
    {
        public int Delay { get; private set; }

        protected AbstractSimulationEvent(int delay)
        {
            this.Delay = delay;
        }
    }
}