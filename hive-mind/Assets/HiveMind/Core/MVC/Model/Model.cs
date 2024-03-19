using UnityEngine;
using Zenject;

namespace HiveMind.MVC.Model
{
    public abstract class Model<TModelSettings>
        where TModelSettings : ScriptableObject
    {
        #region Fields
        protected readonly TModelSettings settings;
        #endregion

        #region Getters
        public TModelSettings Settings => settings;
        #endregion

        #region Constructor
        public Model(string resourcePath)
        {
            Resources.Load<TModelSettings>(resourcePath);
        }
        #endregion

        #region PostConstruct
        [Inject] public abstract void PostConstruct();
        #endregion
    }
}
