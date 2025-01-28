using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers
{
    public sealed class ChangeColorCommand : Command<ChangeColorSignal>
    {
        #region Executes
        public override void Execute(ChangeColorSignal signal) => Debug.Log("Change Color Command Executed!");
        #endregion
    }
}