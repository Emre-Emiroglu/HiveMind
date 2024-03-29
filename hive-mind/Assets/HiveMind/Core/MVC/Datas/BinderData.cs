using Zenject;

namespace HiveMind.Core.MVC.Datas
{
    public readonly struct BinderData
    {
        #region Fields
        private readonly DiContainer container;
        private readonly object key;
        private readonly string assemblyName;
        #endregion

        #region Getters
        public readonly DiContainer Container => container;
        public readonly object Key => key;
        public readonly string AssemblyName => assemblyName;
        #endregion

        #region Constructor
        public BinderData(DiContainer container, object key, string assemblyName)
        {
            this.container = container;
            this.key = key;
            this.assemblyName = assemblyName;
        }
        #endregion
    }
}
