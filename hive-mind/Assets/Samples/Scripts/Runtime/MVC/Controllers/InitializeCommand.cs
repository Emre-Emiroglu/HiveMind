using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers
{
    public sealed class InitializeCommand : Command<InitializeSignal>
    {
        #region Executes
        public override void Execute(InitializeSignal signal) => Debug.Log("MVC Sample Initialize Command Executed!");
        #endregion
    }
}