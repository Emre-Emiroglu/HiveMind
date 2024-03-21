using Zenject;

namespace HiveMind.MVC.Datas
{
    public readonly struct BinderData
    {
        #region Fields
        private readonly DiContainer container;
        private readonly object key;
        #endregion

        #region Getters
        public readonly DiContainer Container => container;
        public readonly object Key => key;
        #endregion

        #region Constructor
        public BinderData(DiContainer container, object key)
        {
            this.container = container;
            this.key = key;
        }
        #endregion
    }
}
