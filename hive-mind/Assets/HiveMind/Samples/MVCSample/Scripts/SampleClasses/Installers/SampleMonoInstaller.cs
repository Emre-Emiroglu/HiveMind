using HiveMind.MVCSample.SampleClasses.Controllers;
using UnityEngine;
using Zenject;

namespace HiveMind.MVCSample.SampleClasses.Installers
{
    public class SampleMonoInstaller : MonoInstaller<SampleMonoInstaller>
    {
        #region Fields
        [SerializeField] private string key = "MVCSample";
        #endregion

        #region Bindings
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            SampleInstaller.Install(Container, new(Container, key));
        }
        #endregion

        #region Cycle
        public override void Start()
        {
            SignalBus signalBus = Container.Resolve<SignalBus>();
            signalBus.Fire(new SampleSignal1());
            signalBus.Fire(new SampleSignal2() { InjectedValue = 99 });
        }
        #endregion
    }
}
