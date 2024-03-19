using System;
using System.Reflection;
using UnityEngine;

namespace HiveMind.MVC.Model
{
    public sealed class ModelBinder
    {
        #region Fields
        private readonly ModelBinderData modelBinderData;
        #endregion

        #region Constructor
        public ModelBinder(ModelBinderData modelBinderData)
        {
            this.modelBinderData = modelBinderData;
        }
        #endregion

        #region Bindings
        public void BindModels()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                ModelAttribute attribute = type.GetCustomAttribute<ModelAttribute>();
                if (attribute != null && attribute.Key == modelBinderData.Key)
                {
                    modelBinderData.Container.Bind(type).AsSingle();
                    Debug.Log($"{type.Name} model is binded");
                }
            }
        }
        #endregion
    }
}
