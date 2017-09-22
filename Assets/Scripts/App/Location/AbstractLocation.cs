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

        public UnityEvent onWorkerJobComplete = new UnityEvent();

        public List<JobSettings> jobs = new List<JobSettings>();
        public List<Job> Jobs { get; private set; }

        public bool HasWorker { get { return jobAssignment != null;  } }

        private bool isAssignable = false;
        private AvailableWorkers availableWorkers;

        private LocationJobIcons.Factory locationJobIconsFactory;
        private LocationJobIcons jobIcons;
        private Player player;
        private JobAssignment jobAssignment;
        private JobAssignment.Factory jobAssignmentFactory;
        
        [Inject]
        public void Construct(LocationJobIcons.Factory locationJobIconsFactory, AvailableWorkers availableWorkers, Player player, JobAssignment.Factory jobAssignmentFactory)
        {
            this.locationJobIconsFactory = locationJobIconsFactory;
            this.availableWorkers = availableWorkers;
            this.player = player;
            this.jobAssignmentFactory = jobAssignmentFactory;
        }

        protected override void Awake()
        {
            RightClickOverStream().Where(isClickOver => isClickOver).Subscribe(value =>
            {
                if (jobAssignment == null)
                    return;

                if (jobAssignment.Worker.IsReclaimable)
                {
                    jobAssignment.Worker.ReclaimWorker();
                }
            });

            Jobs = new List<Job>();
            foreach (var jobSetting in jobs)
            {
                Jobs.Add(new Job(jobSetting));
            }

            base.Awake();
        }

        public void AssignSelectedWorker(Job job)
        {
            if (!isAssignable || jobAssignment != null)
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

            jobAssignment = jobAssignmentFactory.Create(job, worker);

            job.TakeCost(player, worker);

            onWorkerPlacement.Invoke();
        }

        public void ReclaimWorker()
        {
            jobAssignment.Job.GiveCost(player, jobAssignment.Worker);
            RemoveWorker();
            onWorkerReclaimed.Invoke();
        }

        public void CompleteJobAssignment()
        {
            RemoveWorker();
            onWorkerJobComplete.Invoke();
        }

        private void RemoveWorker()
        {
            jobAssignment.Worker.gameObject.SetActive(false);
            jobAssignment.Dispose();
            jobAssignment = null;

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

            jobIcons = locationJobIconsFactory.Create(this, Jobs);

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
