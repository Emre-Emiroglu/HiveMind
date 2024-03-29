using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Datas;
using System;
using System.Linq;
using System.Reflection;

namespace HiveMind.Core.MVC.Binders
{
    public abstract class Binder<TAttribute> where TAttribute : MVCAttribute
    {
        #region Fields
        protected readonly BinderData binderData;
        protected Assembly targetAssembly;
        #endregion

        #region Constructor
        public Binder(BinderData binderData) => this.binderData = binderData;
        #endregion

        #region Bindings
        public virtual void Bind()
        {
            string assemblyName = binderData.AssemblyName;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Assembly assembly = assemblies.First(x => x.FullName.StartsWith(assemblyName));

            targetAssembly = assembly;
        }
        #endregion
    }
}
