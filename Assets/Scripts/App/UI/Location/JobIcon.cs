using App.Jobs;
using App.UI.Events;
using App.UI.Text.Templates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace App.UI.Location
{
    public class JobIcon : AppMonoBehavior
    {
        public UnityEvent onMouseEnter = new UnityEvent();
        public UnityEvent onMouseExit = new UnityEvent();

        public UnityEvent onSelectable = new UnityEvent();
        public UnityEvent onUnselectable = new UnityEvent();

        private LocationJobIcons locationJobIcons;

        public Job Job { get; private set; }

        private ObjectProvider highlightedJobProvider;
        private GlobalEventManager globalEventManager;
        private AvailableWorkers availableWorkers;
        private bool isMouseOver = false;
        private Player player;

        private bool isSelectable = false;

        [Inject]
        public void Construct(
            [Inject(Id="HighlightedJobProvider")]
            ObjectProvider highlightedJobProvider,
            GlobalEventManager globalEventManager,
            AvailableWorkers availableWorkers,
            Player player)
        {
            this.highlightedJobProvider = highlightedJobProvider;
            this.globalEventManager = globalEventManager;
            this.availableWorkers = availableWorkers;
            this.player = player;
        }

        public void Configure(LocationJobIcons jobIcons, Job job)
        {
            Job = job;
            locationJobIcons = jobIcons;
        }

        void OnMouseUpAsButton()
        {
            if (!isSelectable)
                return;

            locationJobIcons.location.AssignSelectedWorker(Job);
        }

        void OnMouseEnter()
        {
            isMouseOver = true;
            highlightedJobProvider.Selected.Value = Job.Settings;
            onMouseEnter.Invoke();
            globalEventManager.onJobIconMouseEnter.Invoke();
        }

        void OnMouseExit()
        {
            MouseExit();
            isMouseOver = false;
        }

        void OnEnable()
        {
            if (Job == null)
            {
                gameObject.SetActive(false);
                return;
            }

            isSelectable = Job.RequirementsMet(player, availableWorkers.SelectedWorker);

            if (isSelectable)
            {
                onSelectable.Invoke();
            }
            else
            {
                onUnselectable.Invoke();
            }
        }

        void OnDisable()
        {
            if (isMouseOver)
            {
                MouseExit();
            }
        }

        protected void MouseExit()
        {
            highlightedJobProvider.Selected.Value = null;
            onMouseExit.Invoke();
            globalEventManager.onJobIconMouseExit.Invoke();
        }

        public class Factory
        {
            private DiContainer container;
            private JobIcon jobIconPrefab;

            public Factory(DiContainer container, JobIcon jobIconPrefab)
            {
                this.container = container;
                this.jobIconPrefab = jobIconPrefab;
            }

            public JobIcon Create(LocationJobIcons jobIcons, Job job)
            {
                jobIconPrefab.gameObject.SetActive(false);
                var result = container.InstantiatePrefabForComponent<JobIcon>(jobIconPrefab, jobIcons.transform);
                result.Configure(jobIcons, job);
                result.gameObject.SetActive(true);
                jobIconPrefab.gameObject.SetActive(true);
                return result;
            }
        }
    }
}