using System;
using App.UI.Providers;
using UnityEngine;
using Zenject;

namespace App.Installer
{
    [CreateAssetMenu(menuName = "Game/Provider Settings")]
    public class ProviderSettingsInstaller : ScriptableObjectInstaller
    {
        public Settings settings;

        public override void InstallBindings()
        {
            Container.Bind<PlayerProvider>().FromInstance(settings.playerProvider);
        }

        [Serializable]
        public class Settings
        {
            public PlayerProvider playerProvider;
        }
    }
}