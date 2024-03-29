using System;

namespace HiveMind.Core.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : MVCAttribute
    {
        #region Constructor
        public ModelAttribute(object key) : base(key) { }
        #endregion
    }
}
