using System;

namespace HiveMind.Core.MVC.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CommandInjectAttribute : MVCAttribute
    {
        #region Constructor
        public CommandInjectAttribute(object key) : base(key) { }
        #endregion
    }
}