using UnityEngine;
using Zenject;

namespace HiveMind.Core.MVC.Runtime.Model
{
    public abstract class Model<TModelSettings> where TModelSettings : ScriptableObject
    {
        #region Fields
        protected readonly TModelSettings _settings;
        #endregion

        #region Getters
        public TModelSettings Settings => _settings;
        #endregion

        #region Constructor
        public Model(string resourcePath)
        {
            if (resourcePath == string.Empty)
            {
                Debug.Log("Resource path can not be null for getting model settings!");
                return;
            }
            _settings = Resources.Load<TModelSettings>(resourcePath);
        }
        #endregion

        #region PostConstruct
        [Inject] public abstract void PostConstruct();
        #endregion
    }
}
