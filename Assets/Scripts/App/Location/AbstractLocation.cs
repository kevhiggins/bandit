using System;
using UnityEngine;
using System.Collections.Generic;
using App.Jobs;
using App.UI.Location;
using App.Worker;
using UniRx;
using UnityEngine.Events;
using Zenject;

namespace App.Location
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AbstractLocation : AppMonoBehavior
    {
        public UnityEvent onAssignable = new UnityEvent();
        public UnityEvent onUnassignable = new UnityEvent();

        public UnityEvent onWorkerPlacement = new UnityEvent();
        public UnityEvent onWorkerReclaimed = new UnityEvent();

        public List<JobSettings> jobs = new List<JobSettings>();
        public List<Job> Jobs { get; private set; }

        public bool HasWorker { get { return worker != null;  } }

        private bool isAssignable = false;
        private AvailableWorkers availableWorkers;

        private AbstractWorker worker;

        private LocationJobIcons.Factory locationJobIconsFactory;
        private LocationJobIcons jobIcons;
        private Job job;
        
        [Inject]
        public void Construct(LocationJobIcons.Factory locationJobIconsFactory, AvailableWorkers availableWorkers)
        {
            this.locationJobIconsFactory = locationJobIconsFactory;
            this.availableWorkers = availableWorkers;
        }

        void Awake()
        {
            RightClickOverStream().Where(isClickOver => isClickOver).Subscribe(value =>
            {
                if (worker == null)
                    return;

                if (worker.IsReclaimable)
                {
                    worker.ReclaimWorker();
                }
            });

            Jobs = new List<Job>();
            foreach (var jobSetting in jobs)
            {
                Jobs.Add(new Job(jobSetting));
            }
        }

        public void AssignSelectedWorker(Job job)
        {
            if (!isAssignable || worker != null)
            {
                return;
            }

            availableWorkers.AssignWorker(this, job);
        }

        public void PlaceWorker(AbstractWorker worker, Job job)
        {
            worker.transform.parent = transform;
            worker.gameObject.SetActive(true);
            worker.transform.localPosition = Vector3.zero;
            this.worker = worker;
            this.job = job;
            onWorkerPlacement.Invoke();
        }

        public void ReclaimWorker()
        {
            worker.gameObject.SetActive(false);
            worker = null;
            job = null;
            onWorkerReclaimed.Invoke();
            if (availableWorkers.HasSelected)
            {
                EnableAssignment();
            }
        }

        public void EnableAssignment()
        {
            if (HasWorker)
                return;

            onAssignable.Invoke();
            var icons = GetJobIcons();
            icons.gameObject.SetActive(true);

            isAssignable = true;
        }

        protected LocationJobIcons GetJobIcons()
        {
            if (jobIcons != null)
            {
                return jobIcons;
            }

            jobIcons = locationJobIconsFactory.Create(this);

            // For each configured job, assign a job settings to a job icon
            if (Jobs.Count > jobIcons.jobIcons.Count)
            {
                throw new Exception(String.Format("Location with name {0} has {1} assigned jobs, when the max allowed Jobs anchors is {2}", name, jobs.Count, jobIcons.jobIcons.Count));
            }

            var count = 0;
            foreach(var job in Jobs)
            {
                var jobIcon = jobIcons.jobIcons[count];
                jobIcon.ConfigureJob(job);
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
