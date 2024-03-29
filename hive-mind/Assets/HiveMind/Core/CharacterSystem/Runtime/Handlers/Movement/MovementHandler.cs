using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Movement
{
    public abstract class MovementHandler: Handler<Vector2, MovementStatus>
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
        public override void Execute(Vector2 inputValue, MovementStatus movementStatus) => base.Execute(inputValue, movementStatus);
        protected override void ExecuteProcess(Vector2 inputValue, MovementStatus movementStatus) { }
        #endregion
    }
}
