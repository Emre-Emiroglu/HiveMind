using HiveMind.MVC.Attributes;
using HiveMind.MVC.Datas;
using System;
using System.Reflection;
using UnityEngine;

namespace HiveMind.MVC.Binders
{
    public sealed class ModelBinder : Binder<ModelAttribute>
    {
        #region Constructor
        public ModelBinder(BinderData binderData) : base(binderData) { }
        #endregion

        #region Bindings
        public override void Bind()
        {
            base.Bind();

            Type[] types = targetAssembly.GetTypes();
            foreach (Type type in types)
            {
                ModelAttribute attribute = type.GetCustomAttribute<ModelAttribute>();

                if (attribute == null || !attribute.Key.Equals(binderData.Key))
                    continue;

                binderData.Container.Bind(type).AsSingle().NonLazy();

                Debug.Log($"Model: {type.Name} is binded!");
            }
        }
        #endregion
    }
}
