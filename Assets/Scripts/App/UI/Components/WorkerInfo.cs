using UnityEngine;
using App.Worker;
using UniRx;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;

namespace App.UI.Components
{
    public class WorkerInfo : MonoBehaviour
    {
        private AvailableWorkers availableWorkers;
        private bool isSelected = false;
        private EventDirector eventDirector;

        public AbstractWorker Worker { get; private set; }

        public UnityEvent onSelected = new UnityEvent();
        public UnityEvent onDeselected = new UnityEvent();
        public UnityEvent onPlacement = new UnityEvent();
        public UnityEvent onReclaimation = new UnityEvent();

        public Button button;
        public GameObject portrait;

        public bool IsSelected
        {
            get { return isSelected;  }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    if (isSelected)
                    {
                        onSelected.Invoke();
                    }
                    else
                    {
                        onDeselected.Invoke();
                    }
                }
            }
        }

        public void Configure(AbstractWorker worker, BanditWorkerSettings workerSetting, AvailableWorkers availableWorkers, EventDirector eventDirector)
        {
            this.eventDirector = eventDirector;
            worker.gameObject.SetActive(false);
            Worker = Instantiate(worker);

            Worker.Init(workerSetting, eventDirector);
            worker.gameObject.SetActive(true);

            Worker.onPlacement.AsObservable().Subscribe(_ =>
            {
                onPlacement.Invoke();
            });

            Worker.onReclaimation.AsObservable().Subscribe(_ =>
            {
                onReclaimation.Invoke();
            });

            this.availableWorkers = availableWorkers;
        }

        public void SignalSelection()
        {
            // Prevent selection if worker is already assigned to location.
            if (Worker.Location.Value != null)
            {
                return;
            }

            availableWorkers.InfoToggled(this);
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void DoPlacement()
        {
            onPlacement.Invoke();
        }

        public void DoReclaimation()
        {
            onReclaimation.Invoke();
        }

        void Start()
        {
            button.onClick.AddListener(SignalSelection);
        }
    }
}