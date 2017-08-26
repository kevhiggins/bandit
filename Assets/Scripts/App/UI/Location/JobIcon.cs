using App.Jobs;
using App.UI.Events;
using App.UI.Text.Templates;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace App.UI.Location
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class JobIcon : MonoBehaviour
    {
        public UnityEvent onMouseEnter = new UnityEvent();
        public UnityEvent onMouseExit = new UnityEvent();

        private JobSettings job;
        private ObjectProvider highlightedJobProvider;
        private GlobalEventManager globalEventManager;

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

        void OnMouseEnter()
        {
            highlightedJobProvider.Selected.Value = job;
            onMouseEnter.Invoke();
            globalEventManager.onJobIconMouseEnter.Invoke();
        }

        void OnMouseExit()
        {
            highlightedJobProvider.Selected.Value = null;
            onMouseExit.Invoke();
            globalEventManager.onJobIconMouseExit.Invoke();
        }
    }
}