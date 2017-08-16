using UnityEngine;
using System.Collections;
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

        private ReactiveProperty<AbstractLocation> location;
        public ReadOnlyReactiveProperty<AbstractLocation> Location { get; private set; }

        private bool isInitialized = false;

        public void Init()
        {
            if (isInitialized)
                return;

            location = new ReactiveProperty<AbstractLocation>();
            Location = location.ToReadOnlyReactiveProperty();

            isInitialized = true;
        }

        public void PlaceWorker(AbstractLocation location)
        {
            this.location.Value = location;
            location.PlaceWorker(this);
            onPlacement.Invoke();
        }

        public void ReclaimWorker()
        {
            location.Value.ReclaimWorker();
            location.Value = null;
            onReclaimation.Invoke();
        }
    }
}