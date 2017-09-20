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

        public AbstractWorker Worker { get; private set; }

        public UnityEvent onSelected = new UnityEvent();
        public UnityEvent onDeselected = new UnityEvent();
        public UnityEvent onPlacement = new UnityEvent();
        public UnityEvent onReturn = new UnityEvent();

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

        public void Configure(AbstractWorker workerPrefab, BanditWorkerSettings workerSetting, AvailableWorkers availableWorkers, EventDirector eventDirector)
        {
            workerPrefab.gameObject.SetActive(false);
            Worker = Instantiate(workerPrefab);

            Worker.Init(workerSetting, eventDirector);
            workerPrefab.gameObject.SetActive(true);

            Worker.onPlacement.AsObservable().Subscribe(_ =>
            {
                onPlacement.Invoke();
            });

            var onReclamation = Worker.onReclaimation.AsObservable();
            var onJobComplete = Worker.onJobAssignmentComplete.AsObservable();
            var onBanditReturned = onReclamation.Merge(onJobComplete);

            onBanditReturned.Subscribe(_ =>
            {
                onReturn.Invoke();
            });

            this.availableWorkers = availableWorkers;
        }

        public void SignalSelection()
        {
            // Prevent selection if workerPrefab is already assigned to location.
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

        void Start()
        {
            button.onClick.AddListener(SignalSelection);
        }
    }
}