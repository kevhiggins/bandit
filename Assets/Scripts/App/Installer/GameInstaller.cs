﻿using System.Diagnostics;
using App.UI.Events;
using Zenject;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace App.Installer
{
    public class GameInstaller : MonoInstaller
    {
        public EventDirector eventDirector;

        public override void InstallBindings()
        {
            Container.Bind<TownManager>().AsSingle();
            Container.Bind<GlobalEventManager>().FromInstance(eventDirector.globalEvents);
        }
    }
}