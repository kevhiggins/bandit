using System;
using UnityEngine;
using System.Collections;
using App.Worker;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace App.Location
{
    public class AbstractLocation : MonoBehaviour
    {
        public UnityEvent onAssignable;
        public UnityEvent onUnassignable;

        public bool HasWorker { get { return worker != null;  } }

        private bool isAssignable = false;
        private AvailableWorkers availableWorkers;

        private AbstractWorker worker;

        void Awake()
        {
            availableWorkers = FindObjectOfType<AvailableWorkers>();
            if (availableWorkers == null)
            {
                throw new Exception("Could not find available workers component.");
            }
        }

        void OnMouseUpAsButton()
        {
            if (!isAssignable || worker != null)
            {
                return;
            }

            var selectedWorker = availableWorkers.AssignWorker(this);

            if (selectedWorker == null)
            {
                throw new Exception("No worker selected. Location should not be assignable without a selected worker.");
            }

            var workerObject = Object.Instantiate(selectedWorker, this.transform);
            worker = workerObject.GetComponent<AbstractWorker>();
            if (worker == null)
            {
                throw new Exception("Could not get worker script from worker object.");
            }
        }

        public void EnableAssignment()
        {
            if (HasWorker)
                return;

            this.onAssignable.Invoke();
            isAssignable = true;
        }

        public void DisableAssignment()
        {
            this.onUnassignable.Invoke();
            isAssignable = false;
        }
    }
}
