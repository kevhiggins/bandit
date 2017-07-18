using UnityEngine;
using App.Worker;
using TMPro;
using UnityEngine.UI;

namespace App.UI.Components
{
    public class WorkerInfo : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Button assignmentButton;
        private AvailableWorkers availableWorkers;

        public AbstractWorker Worker { get; private set; }

        public void Configure(AbstractWorker worker, AvailableWorkers availableWorkers)
        {
            this.Worker = worker;
            this.availableWorkers = availableWorkers;
        }

        public void ToggleAssign()
        {
            assignmentButton.image.color = Color.gray;
            availableWorkers.InfoToggled(this);

        }

        public void Deselect()
        {
            assignmentButton.image.color = Color.white;
        }

        void Start()
        {
            this.nameText.text = Worker.workerName;
            assignmentButton.onClick.AddListener(ToggleAssign);          
        }
    }
}