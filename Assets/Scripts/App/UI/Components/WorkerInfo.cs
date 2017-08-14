using UnityEngine;
using App.Worker;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;
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
        public Button button;

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

        public void Configure(AbstractWorker worker, AvailableWorkers availableWorkers)
        {
            Worker = worker;
            this.availableWorkers = availableWorkers;
        }

        public void ToggleAssign()
        {
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

        void Start()
        {
            button.onClick.AddListener(ToggleAssign);
        }
    }
}