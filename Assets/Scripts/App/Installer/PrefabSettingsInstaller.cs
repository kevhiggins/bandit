using System;
using App.Simulation;
using App.UI.Location;
using UnityEngine;
using Zenject;

namespace App.Installer
{
    [CreateAssetMenu(menuName = "Game/Prefab Settings")]
    public class PrefabSettingsInstaller : ScriptableObjectInstaller<PrefabSettingsInstaller>
    {
        public PrefabSettings prefabSettings;

        public override void InstallBindings()
        {
            Container.Bind<LocationJobIcons>().FromInstance(prefabSettings.locationJobIcons);
            Container.Bind<LocationJobIcons.Factory>().AsSingle();

            //Container.BindFactory<LocationJobIcons, LocationJobIcons.Factory>()
            //.FromComponentInNewPrefab(prefabSettings.locationJobIcons);
        }

        [Serializable]
        public class PrefabSettings
        {
            public LocationJobIcons locationJobIcons;
        }
    }
}