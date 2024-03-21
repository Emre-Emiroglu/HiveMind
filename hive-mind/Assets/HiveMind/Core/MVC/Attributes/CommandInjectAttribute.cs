using System;

namespace HiveMind.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInjectAttribute : MVCAttribute
    {
        #region Constructor
        public CommandInjectAttribute(object key = null) : base(key) { }
        #endregion
    }
}