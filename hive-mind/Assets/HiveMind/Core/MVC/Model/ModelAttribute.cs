using System;

namespace HiveMind.MVC.Model
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelAttribute : Attribute
    {
        #region Fields
        private readonly string key;
        #endregion

        #region Getters
        public string Key => key;
        #endregion

        #region Constructor
        public ModelAttribute(string key = "")
        {
            this.key = key;
        }
        #endregion
    }
}
