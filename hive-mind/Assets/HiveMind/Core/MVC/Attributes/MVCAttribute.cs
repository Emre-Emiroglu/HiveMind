using System;

namespace HiveMind.MVC.Attributes
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
        public MVCAttribute(object key = null) => this.key = key;
        #endregion
    }
}
