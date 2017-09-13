using System;
using System.Collections.Generic;
using App.Jobs;
using App.Location;
using UnityEngine;
using Zenject;

namespace App.UI.Location
{
    public class LocationJobIcons : MonoBehaviour
    {
        [NonSerialized]
        public AbstractLocation location;

        private List<Job> jobs;
        private List<JobIcon> jobIcons = new List<JobIcon>();
        private JobIcon.Factory jobIconFactory;


        [Inject]
        public void Construct(JobIcon.Factory jobIconFactory)
        {
            this.jobIconFactory = jobIconFactory;
        }

        public void Configure(AbstractLocation location, List<Job> jobs)
        {
            this.location = location;
            this.jobs = jobs;

            Render();
        }

        private void Render()
        {
            foreach (var job in jobs)
            {
                var jobIcon = jobIconFactory.Create(this, job);
                jobIcons.Add(jobIcon);
            }
        }

        public class Factory
        {
            private DiContainer container;
            private LocationJobIcons locationJobIcons;

            public Factory(DiContainer container, LocationJobIcons locationJobIcons)
            {
                this.container = container;
                this.locationJobIcons = locationJobIcons;
            }

            public LocationJobIcons Create(AbstractLocation location, List<Job> jobs)
            {
                locationJobIcons.gameObject.SetActive(false);
                var result = container.InstantiatePrefabForComponent<LocationJobIcons>(locationJobIcons, location.transform);
                result.Configure(location, jobs);
                locationJobIcons.gameObject.SetActive(true);
                return result;
            }
        }
    }
}