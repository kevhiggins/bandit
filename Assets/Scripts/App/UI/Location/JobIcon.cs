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
        public LocationJobIcons locationJobIcons;

        private JobSettings job;
        private ObjectProvider highlightedJobProvider;
        private GlobalEventManager globalEventManager;
        private bool isMouseOver = false;

        [Inject]
        public void Construct(
            [Inject(Id="HighlightedJobProvider")]
            ObjectProvider highlightedJobProvider,
            GlobalEventManager globalEventManager)
        {
            this.highlightedJobProvider = highlightedJobProvider;
            this.globalEventManager = globalEventManager;
        }

        public void ConfigureJob(JobSettings job)
        {
            this.job = job;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = job.icon;
        }

        void OnMouseUpAsButton()
        {
            locationJobIcons.location.AssignSelectedWorker(job);
        }

        void OnMouseEnter()
        {
            isMouseOver = true;
            highlightedJobProvider.Selected.Value = job;
            onMouseEnter.Invoke();
            globalEventManager.onJobIconMouseEnter.Invoke();
        }

        void OnMouseExit()
        {
            MouseExit();
            isMouseOver = false;
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