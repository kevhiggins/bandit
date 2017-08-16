using App.UI.Events;
using Zenject;

namespace App.Installer
{
    public class GameInstaller : MonoInstaller
    {
        public EventDirector eventDirector;
        public AvailableWorkers availableWorkers;

        public override void InstallBindings()
        {
            Container.Bind<TownManager>().AsSingle();
            Container.Bind<GlobalEventManager>().FromInstance(eventDirector.globalEvents);
            Container.Bind<EventDirector>().FromInstance(eventDirector);
            Container.Bind<AvailableWorkers>().FromInstance(availableWorkers);
        }
    }
}