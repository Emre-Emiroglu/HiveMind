using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Input
{
    public sealed class GamepadInputHandler : InputHandler
    {
        #region Constructor
        public GamepadInputHandler(InputData inputData) : base(inputData) { }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public override void Execute() => base.Execute();
        protected override void ExecuteProcess() => base.ExecuteProcess();
        #endregion
    }
}
