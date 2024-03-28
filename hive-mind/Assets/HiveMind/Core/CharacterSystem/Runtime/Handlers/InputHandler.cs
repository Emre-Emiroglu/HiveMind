using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class InputHandler: Handler
    {
        #region ReadonlyFields
        private readonly InputData inputData;
        private readonly InputActionAsset inputActionAsset;
        private readonly InputControlScheme? controlScheme;
        private readonly InputActionMap actionMap;
        private readonly InputAction movementAction;
        #endregion

        #region Fields
        private Vector2 movementInputValue;
        #endregion

        #region Getters
        public Vector2 MovementInputValue => movementInputValue;
        #endregion

        #region Constructor
        public InputHandler(InputData inputData) : base()
        {
            this.inputData = inputData;
            inputActionAsset = this.inputData.InputActionAsset;

            controlScheme = inputActionAsset.FindControlScheme(this.inputData.ControlSchemeName);
            if (controlScheme == null)
                return;

            actionMap = inputActionAsset.FindActionMap(this.inputData.ActionMapName);
            movementAction = actionMap.FindAction(this.inputData.MovementActionName);

            movementAction.started += OnMovementStarted;
            movementAction.performed += OnMovementPreformed;
            movementAction.canceled += OnMovementCanceled;
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            base.Dispose();

            if (controlScheme == null)
                return;

            movementAction.started -= OnMovementStarted;
            movementAction.performed -= OnMovementPreformed;
            movementAction.canceled -= OnMovementCanceled;
        }
        #endregion

        #region Set
        public override void SetEnableStatus(bool isEnable)
        {
            base.SetEnableStatus(isEnable);

            if (isEnable)
                inputActionAsset.Enable();
            else
                inputActionAsset.Disable();
        }
        #endregion

        #region Executes
        public override void Execute() => base.Execute();
        protected override void ExecuteProcess() { }
        #endregion

        #region Receivers
        private void OnMovementStarted(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        private void OnMovementPreformed(InputAction.CallbackContext context) => movementInputValue = context.ReadValue<Vector2>();
        private void OnMovementCanceled(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        #endregion
    }
}
