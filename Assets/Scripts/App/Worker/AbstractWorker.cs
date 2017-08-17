using System;
using UnityEngine;
using App.Location;
using UniRx;
using UnityEngine.Events;

namespace App.Worker
{
    public class AbstractWorker : MonoBehaviour
    {
        public string workerName;
        public int stamina = 5;
        public GameObject portrait;

        public UnityEvent onPlacement = new UnityEvent();
        public UnityEvent onReclaimation = new UnityEvent();
        public UnityEvent onNotReclaimable = new UnityEvent();

        private ReactiveProperty<AbstractLocation> location;
        public ReadOnlyReactiveProperty<AbstractLocation> Location { get; private set; }
        public bool IsReclaimable { get; private set; }

        private bool isInitialized = false;
        private EventDirector eventDirector;

        private IDisposable isSimulatingSubscription;

        public void Init(EventDirector eventDirector)
        {
            if (isInitialized)
                return;

            IsReclaimable = false;
            location = new ReactiveProperty<AbstractLocation>();
            Location = location.ToReadOnlyReactiveProperty();
            this.eventDirector = eventDirector;

            isInitialized = true;
        }

        public void PlaceWorker(AbstractLocation location)
        {
            IsReclaimable = true;

            // Wait for IsSimulating to be true, and then prevent the ability to remove the bandit from the board.
            isSimulatingSubscription = eventDirector.IsSimulating.First(isSimulating => isSimulating).Subscribe(isSimulating =>
            {
                IsReclaimable = false;
                onNotReclaimable.Invoke();
            });

            this.location.Value = location;
            location.PlaceWorker(this);
            onPlacement.Invoke();
        }

        public void ReclaimWorker()
        {
            if (!IsReclaimable)
                throw new Exception("Cannot reclaim worker.");

            if (isSimulatingSubscription != null)
            {
                isSimulatingSubscription.Dispose();
            }

            location.Value.ReclaimWorker();
            location.Value = null;
            onReclaimation.Invoke();
        }
    }
}