using App.Jobs;
using UnityEngine;

namespace App.UI.Location
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class JobIcon : MonoBehaviour
    {
        private JobSettings job;

        public void ConfigureJob(JobSettings job)
        {
            this.job = job;
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = job.icon;
        }
    }
}