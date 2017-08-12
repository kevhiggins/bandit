using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace App.UI.Location
{
    public class LocationJobIcons : MonoBehaviour
    {
        public List<JobIcon> jobIcons = new List<JobIcon>();

        public class Factory
        {
            private DiContainer container;
            private LocationJobIcons locationJobIcons;

            public Factory(DiContainer container, LocationJobIcons locationJobIcons)
            {
                this.container = container;
                this.locationJobIcons = locationJobIcons;
            }

            public LocationJobIcons Create(Transform parentTransform)
            {
                locationJobIcons.gameObject.SetActive(false);
                var result = container.InstantiatePrefabForComponent<LocationJobIcons>(locationJobIcons, parentTransform);
                locationJobIcons.gameObject.SetActive(true);
                return result;
            }
        }
    }
}