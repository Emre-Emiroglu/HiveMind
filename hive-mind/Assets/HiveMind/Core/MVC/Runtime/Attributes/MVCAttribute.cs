using System;

namespace HiveMind.Core.MVC.Runtime.Attributes
{
    public abstract class MVCAttribute : Attribute
    {
        #region Fields
        protected readonly object key;
        #endregion

        #region Getters
        public object Key => key;
        #endregion

        #region Constructor
        public MVCAttribute(object key) => this.key = key;
        #endregion
    }
}
