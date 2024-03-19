using Zenject;

namespace HiveMind.MVC.Model
{
    public readonly struct ModelBinderData
    {
        #region Fields
        private readonly DiContainer container;
        private readonly string key;
        #endregion

        #region Getters
        public readonly DiContainer Container => container;
        public readonly string Key => key;
        #endregion

        #region Constructor
        public ModelBinderData(DiContainer container, string key = "")
        {
            this.container = container;
            this.key = key;
        }
        #endregion
    }
}
