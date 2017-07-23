using System.Collections.Generic;

namespace App.Simulation
{
    // Sort ISimulationEvent by delay ascending.
    public class SimulationEventComparer : IComparer<ISimulationEvent>
    {
        
        public int Compare(ISimulationEvent x, ISimulationEvent y)
        {
            if (x.Delay > y.Delay)
            {
                return 1;
            }

            if (x.Delay < y.Delay)
            {
                return -1;
            }

            return 0;
        }
    }
}