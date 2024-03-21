using HiveMind.MVC.Controllers;
using UnityEngine;

namespace HiveMind.MVCSample.SampleClasses.Controllers
{
    public class SampleCommand1 : Command
    {
        #region Executes
        public override void Execute() => Debug.Log("Sample Command 1 executed!");
        #endregion
    }
}
