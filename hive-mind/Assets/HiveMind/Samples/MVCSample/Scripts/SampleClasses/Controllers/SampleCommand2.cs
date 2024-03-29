using HiveMind.Core.MVC.Attributes;
using HiveMind.Core.MVC.Controllers;
using UnityEngine;

namespace HiveMind.Samples.MVCSample.SampleClasses.Controllers
{
    public class SampleCommand2 : Command
    {
        #region Fields
        [CommandInject("Sample Command 2")] private int injectedValue;
        #endregion

        #region Executes
        public override void Execute() => Debug.Log($"Sample Command 2 executed! with injected value: {injectedValue}");
        #endregion
    }
}
