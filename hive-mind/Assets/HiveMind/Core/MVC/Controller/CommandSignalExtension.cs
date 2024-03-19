using Zenject;

namespace HiveMind.MVC.Controller
{
    public static class CommandSignalExtensions
    {
        public static BinderSignalToCommand<TSignal> BindSignal<TSignal>(this DiContainer container)
        {
            var signalBindInfo = new SignalBindingBindInfo(typeof(TSignal));

            return new BinderSignalToCommand<TSignal>(container, signalBindInfo);
        }
    }
}
