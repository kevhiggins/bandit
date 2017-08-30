using System;
using System.Collections.Generic;
using App.Location;
using UnityEngine;
using Zenject;

namespace App.UI.Location
{
    public class LocationJobIcons : MonoBehaviour
    {
        public List<JobIcon> jobIcons = new List<JobIcon>();

        [NonSerialized]
        public AbstractLocation location;

        public class Factory
        {
            private DiContainer container;
            private LocationJobIcons locationJobIcons;

            public Factory(DiContainer container, LocationJobIcons locationJobIcons)
            {
                this.container = container;
                this.locationJobIcons = locationJobIcons;
            }

            public LocationJobIcons Create(AbstractLocation location)
            {
                locationJobIcons.gameObject.SetActive(false);
                var result = container.InstantiatePrefabForComponent<LocationJobIcons>(locationJobIcons, location.transform);
                result.location = location;
                locationJobIcons.gameObject.SetActive(true);
                return result;
            }
        }
    }
}