using System;
using UnityEngine;
using System.Collections.Generic;
using App.Jobs;
using App.UI.Location;
using App.Worker;
using UnityEngine.Events;
using Zenject;
using Object = UnityEngine.Object;

namespace App.Location
{
    public class AbstractLocation : MonoBehaviour
    {
        public UnityEvent onAssignable;
        public UnityEvent onUnassignable;

        public List<JobSettings> jobs = new List<JobSettings>();

        public bool HasWorker { get { return worker != null;  } }

        private bool isAssignable = false;
        private AvailableWorkers availableWorkers;

        private AbstractWorker worker;

        private LocationJobIcons.Factory locationJobIconsFactory;
        private LocationJobIcons jobIcons;

        void Awake()
        {
            availableWorkers = FindObjectOfType<AvailableWorkers>();
            if (availableWorkers == null)
            {
                throw new Exception("Could not find available workers component.");
            }
        }

        [Inject]
        public void Construct(LocationJobIcons.Factory locationJobIconsFactory)
        {
            this.locationJobIconsFactory = locationJobIconsFactory;
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

            onAssignable.Invoke();
            var icons = GetJobIcons();
            icons.gameObject.SetActive(true);
         //   icons.gameObject.SetActive(false);
        //    icons.enabled = true;

            isAssignable = true;
        }

        protected LocationJobIcons GetJobIcons()
        {
            if (jobIcons != null)
            {
                return jobIcons;
            }

            jobIcons = locationJobIconsFactory.Create(transform);

            // For each configured job, assign a job settings to a job icon
            if (jobs.Count > jobIcons.jobIcons.Count)
            {
                throw new Exception(String.Format("Location with name {0} has {1} assigned jobs, when the max allowed Jobs anchors is {2}", name, jobs.Count, jobIcons.jobIcons.Count));
            }

            var count = 0;
            foreach(var jobSetting in jobs)
            {
                var jobIcon = jobIcons.jobIcons[count];
                jobIcon.ConfigureJob(jobSetting);
                count++;
            }

            return jobIcons;
        }

        public void DisableAssignment()
        {
            onUnassignable.Invoke();
            var icons = GetJobIcons();
            icons.gameObject.SetActive(false);
            isAssignable = false;
        }
    }
}
