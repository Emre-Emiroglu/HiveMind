using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public abstract class MovementHandler: Handler<Vector2>
    {
        #region ReadonlyFields
        protected readonly MovementData movementData;
        #endregion

        #region Constructor
        public MovementHandler(MovementData movementData) : base()
        {
            this.movementData = movementData;
        }
        #endregion

        #region Dispose
        public override void Dispose() => base.Dispose();
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable) => base.SetEnableStatus(isEnable);
        #endregion

        #region Executes
        public override void Execute(Vector2 inputValue) => base.Execute(inputValue);
        protected override void ExecuteProcess(Vector2 inputValue) { }
        #endregion
    }
}
