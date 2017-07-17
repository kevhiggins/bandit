using UnityEngine;
using App.Worker;
using TMPro;
using UnityEngine.Experimental.UIElements;

namespace App.UI.Components
{
    public class WorkerInfo : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        private AbstractWorker worker;

        public void Configure(AbstractWorker worker)
        {
            this.worker = worker;
        }

        public void ToggleAssign(Button button)
        {
            
        }

        void Start()
        {
            this.nameText.text = worker.workerName;
        }
    }
}