using App.Jobs;
using App.UI.Events;
using App.UI.Text.Templates;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace App.UI.Location
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class JobIcon : AppMonoBehavior
    {
        public UnityEvent onMouseEnter = new UnityEvent();
        public UnityEvent onMouseExit = new UnityEvent();

        public UnityEvent onSelectable = new UnityEvent();
        public UnityEvent onUnselectable = new UnityEvent();

        public LocationJobIcons locationJobIcons;

        private Job job;
        private ObjectProvider highlightedJobProvider;
        private GlobalEventManager globalEventManager;
        private AvailableWorkers availableWorkers;
        private bool isMouseOver = false;

        [Inject]
        public void Construct(
            [Inject(Id="HighlightedJobProvider")]
            ObjectProvider highlightedJobProvider,
            GlobalEventManager globalEventManager,
            AvailableWorkers availableWorkers)
        {
            this.highlightedJobProvider = highlightedJobProvider;
            this.globalEventManager = globalEventManager;
            this.availableWorkers = availableWorkers;
        }

        public void ConfigureJob(Job job)
        {
            this.job = job;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = job.Settings.icon;
        }

        void OnMouseUpAsButton()
        {
            locationJobIcons.location.AssignSelectedWorker(job);
        }

        void OnMouseEnter()
        {
            isMouseOver = true;
            highlightedJobProvider.Selected.Value = job.Settings;
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
            //availableWorkers.worker
            
            // Add method to JobSettings that takes a worker and checks if it is valid;

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
    }
}