using App.Jobs;
using App.UI.Text.Templates;
using UnityEngine;
using Zenject;

namespace App.UI.Location
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class JobIcon : MonoBehaviour
    {
        private JobSettings job;
        private ObjectProvider highlightedJobProvider;

        [Inject]
        public void Construct(
            [Inject(Id="HighlightedJobProvider")]
            ObjectProvider highlightedJobProvider)
        {
            this.highlightedJobProvider = highlightedJobProvider;
        }

        public void ConfigureJob(JobSettings job)
        {
            this.job = job;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = job.icon;
        }

        void OnMouseEnter()
        {
            Debug.Log(highlightedJobProvider);
            highlightedJobProvider.Selected = job;
        }

        void OnMouseExit()
        {
            highlightedJobProvider.Selected = null;
        }
    }
}