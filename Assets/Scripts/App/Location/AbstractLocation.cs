using System;
using UnityEngine;
using System.Collections.Generic;
using App.Jobs;
using App.UI.Location;
using App.Worker;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;
using Zenject;
using Object = UnityEngine.Object;

namespace App.Location
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AbstractLocation : MonoBehaviour
    {
        public UnityEvent onAssignable = new UnityEvent();
        public UnityEvent onUnassignable = new UnityEvent();

        public UnityEvent onWorkerPlacement = new UnityEvent();
        public UnityEvent onWorkerReclaimed = new UnityEvent();

        public List<JobSettings> jobs = new List<JobSettings>();

        public bool HasWorker { get { return worker != null;  } }

        private bool isAssignable = false;
        private AvailableWorkers availableWorkers;

        private AbstractWorker worker;

        private LocationJobIcons.Factory locationJobIconsFactory;
        private LocationJobIcons jobIcons;
        
        [Inject]
        public void Construct(LocationJobIcons.Factory locationJobIconsFactory, AvailableWorkers availableWorkers)
        {
            this.locationJobIconsFactory = locationJobIconsFactory;
            this.availableWorkers = availableWorkers;
        }

        void Awake()
        {


            // right click happened
            // Mouse is over the current game object
            // We have a frame with a right click followed by a frame without a right click down

            var rightClickOverStream = RightClickOverStream();

            rightClickOverStream.Where(isClickOver => isClickOver).Subscribe(value =>
            {
                if (worker == null)
                    return;

                if (worker.IsReclaimable)
                {
                    worker.ReclaimWorker();
                }
            });
        }

        void OnMouseUpAsButton()
        {
            if (!isAssignable || worker != null)
            {
                return;
            }

            availableWorkers.AssignWorker(this);
        }

        public void PlaceWorker(AbstractWorker worker)
        {
            worker.transform.parent = transform;
            worker.gameObject.SetActive(true);
            worker.transform.localPosition = Vector3.zero;
            this.worker = worker;
            onWorkerPlacement.Invoke();
        }

        public void ReclaimWorker()
        {
            worker.gameObject.SetActive(false);
            worker = null;
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

        protected IObservable<bool> RightClickOverStream()
        {
            // Stream of the current state of the right mouse button each frame update.
            var rightMouseStream = Observable.EveryUpdate()
                .Select(_ => Input.GetKey(KeyCode.Mouse1));

            // Only be true on right mouse up after down.
            var rightMouseUpStream = rightMouseStream.Pairwise((prev, current) => prev && !current);


            // Streams a true value when the mouse enters the location.
            var mouseEnterStream = this.OnMouseEnterAsObservable().Select(_ => true);

            // Streams a false value when the mouse exists the location
            var mouseExitStream = this.OnMouseExitAsObservable().Select(_ => false);

            // Streams boolean values for whehter or not the mouse is over this location.
            var mouseOverStream = mouseEnterStream.Merge(mouseExitStream);

            return mouseOverStream.CombineLatest(rightMouseUpStream.DistinctUntilChanged(), (isMouseOver, isMouseUp) => isMouseOver && isMouseUp).DistinctUntilChanged();
        }
    }
}
