using System;

namespace HiveMind.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : MVCAttribute
    {
        #region Constructor
        public ModelAttribute(object key = null) : base(key) { }
        #endregion
    }
}
