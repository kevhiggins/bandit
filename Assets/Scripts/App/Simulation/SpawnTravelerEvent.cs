using App.Graph;
using App.Location;
using App.Unit;
using RSG;
using UnityEngine;

namespace App.Simulation
{
    public class SpawnTravelerEvent : ISimulationEvent
    {
        private Traveler traveler;
        private Town startTown;
        private Town endTown;
        private float delay;

        public float Delay { get { return delay; } }

        public SpawnTravelerEvent(Traveler traveler, Town startTown, Town endTown, float delay)
        {
            this.traveler = traveler;
            this.startTown = startTown;
            this.endTown = endTown;
            this.delay = delay;
        }

        public IPromise Start()
        {
            var travelerGameObject = Object.Instantiate(traveler, startTown.transform.position, Quaternion.identity);
            var graphNavigator = travelerGameObject.GetComponent<GraphNavigator>();

            graphNavigator.SetTargetNode(startTown.Node);

            var travelerInstance = travelerGameObject.GetComponent<Traveler>();

            travelerInstance.SourceTown = startTown;
            return travelerInstance.MoveToTown(endTown);
        }
    }
}