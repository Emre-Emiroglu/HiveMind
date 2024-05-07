using HiveMind.Core.MVC.Runtime.Attributes;
using HiveMind.Core.MVC.Runtime.Datas;
using HiveMind.Core.MVC.Runtime.Views;
using System;
using System.Reflection;
using UnityEngine;

namespace HiveMind.Core.MVC.Runtime.Binders
{
    public sealed class MediationBinder : Binder<MediatorAttribute>
    {
        #region Constructor
        public MediationBinder(BinderData binderData) : base(binderData) { }
        #endregion

        #region Bindings
        public override void Bind()
        {
            base.Bind();

            GameObject[] sceneObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var sceneObject in sceneObjects)
            {
                var views = sceneObject.GetComponentsInChildren<View>();
                foreach (var view in views)
                {
                    Type viewType = view.GetType();

                    if (binderData.Container.HasBinding(viewType))
                        continue;

                    binderData.Container.Bind(viewType).FromInstance(view);

                    Debug.Log($"View: {viewType.Name} is binded!");
                }
            }

            Type[] types = targetAssembly.GetTypes();
            foreach (Type type in types)
            {
                MediatorAttribute attribute = type.GetCustomAttribute<MediatorAttribute>();

                if (attribute == null || !attribute.Key.Equals(binderData.Key))
                    continue;

                binderData.Container.BindInterfacesAndSelfTo(type).AsSingle().NonLazy();

                Debug.Log($"Mediator: {type.Name} is binded!");
            }
        }
        #endregion
    }
}
