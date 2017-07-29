using Zenject;

namespace App.Installer
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<TownManager>().AsSingle();
        }
    }
}