using Sources.Views;
using UnityEngine;

namespace Sources.Installers
{
    public class PrefabInstaller : Zenject.MonoInstaller
    {
        [SerializeField] private GameObject floorPlane;
        [SerializeField] private GameObject floorGrid;
        [SerializeField] private GameObject builder;
        [SerializeField] private GameObject recoursesRepository;
        
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromComponentInHierarchy().AsCached().NonLazy();
            Container.Bind<GridView>().FromComponentOn(floorGrid).AsSingle().NonLazy();
            Container.Bind<Ground>().FromComponentOn(floorPlane).AsSingle().NonLazy();
            Container.Bind<Builder>().FromComponentOn(builder).AsSingle().NonLazy();
            Container.Bind<RecoursesRepository>().FromComponentOn(recoursesRepository).AsSingle().NonLazy();
        }
    }
}