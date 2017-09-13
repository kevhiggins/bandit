using System;
using App.Simulation;
using App.UI.Location;
using App.UI.Text.Templates;
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

            Container.Bind<JobIcon>().FromInstance(prefabSettings.jobIcon);
            Container.Bind<JobIcon.Factory>().AsSingle();

            Container.Bind<ObjectProvider>().WithId("HighlightedJobProvider").FromInstance(prefabSettings.highlightedJobProvider);
        }

        [Serializable]
        public class PrefabSettings
        {
            public LocationJobIcons locationJobIcons;
            public JobIcon jobIcon;
            public ObjectProvider highlightedJobProvider;
        }
    }
}