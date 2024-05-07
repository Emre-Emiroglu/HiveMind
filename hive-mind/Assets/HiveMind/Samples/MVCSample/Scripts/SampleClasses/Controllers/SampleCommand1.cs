using HiveMind.Core.MVC.Runtime.Controllers;
using UnityEngine;

namespace HiveMind.Samples.MVCSample.SampleClasses.Controllers
{
    public class SampleCommand1 : Command
    {
        #region Executes
        public override void Execute() => Debug.Log("Sample Command 1 executed!");
        #endregion
    }
}
