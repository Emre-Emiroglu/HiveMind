using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handlers.CrossScene;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
using UnityEngine;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
{
    public sealed class CrossSceneMonoInstaller : MonoInstaller
    {
        #region Fields
        [Header("Factories Fields")]
        [SerializeField] private Transform currencyTrailParent;
        [SerializeField] private GameObject currencyTrailPrefab;
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            Container.Install<CrossSceneModelInstaller>();
            Container.Install<CrossSceneMediationInstaller>();
            Container.Install<CrossSceneSignalInstaller>();

            Container.BindInterfacesAndSelfTo<CurrencyTrailSpawnHandler>().AsSingle().NonLazy();

            Container.BindFactory<CurrencyTrailData, CurrencyTrailMediator, CurrencyTrailFactory>()
              .FromPoolableMemoryPool<CurrencyTrailData, CurrencyTrailMediator, CurrencyTrailPool>
              (poolBinder => poolBinder
                  .WithInitialSize(5)
                  .FromSubContainerResolve()
                  .ByNewPrefabInstaller<CurrencyTrailInstaller>(currencyTrailPrefab)
                  .UnderTransform(currencyTrailParent)
              );
        }
        #endregion
    }
}
