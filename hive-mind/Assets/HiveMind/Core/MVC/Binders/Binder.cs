using HiveMind.MVC.Attributes;
using HiveMind.MVC.Datas;
using System;
using System.Reflection;
using UnityEngine;
using Zenject;

namespace HiveMind.MVC.Binders
{
    public abstract class Binder<TAttribute> where TAttribute : MVCAttribute
    {
        #region Fields
        protected readonly BinderData binderData;
        #endregion

        #region Constructor
        public Binder(BinderData binderData) => this.binderData = binderData;
        #endregion

        #region Bindings
        public virtual void Bind()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();

            DiContainer container = binderData.Container;
            object key = binderData.Key;

            foreach (Type type in types)
            {
                TAttribute attribute = type.GetCustomAttribute<TAttribute>();

                if (attribute != null && attribute.Key == key)
                {
                    container.Bind(type).AsSingle().NonLazy();
                    Debug.Log($"{type.Name} is binded");
                }
            }
        }
        #endregion
    }
}
