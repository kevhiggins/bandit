using App.Graph;
using App.Location;
using App.Unit;
using RSG;
using UnityEngine;

namespace App.Simulation
{
    [System.Serializable]
    public class SpawnTravelerEvent : ISimulationEvent
    {
        public Traveler traveler;
        public Town startTown;
        public Town endTown;
        public int delay;

        public float Delay { get { return delay; } }
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