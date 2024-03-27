using HiveMind.CharacterSystem.Runtime.Datas.ValueObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HiveMind.CharacterSystem.Runtime.Handlers
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
        private Vector2 rotationInputValue;
        #endregion

        #region Getters
        public Vector2 MovementInputValue => movementInputValue;
        public Vector2 RotationInputValue => rotationInputValue;
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

            movementAction.canceled += OnMovementCanceled;
            movementAction.started += OnMovementStarted;
            movementAction.performed += OnMovementPreformed;
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

        #region Receivers
        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            movementInputValue = Vector2.zero;
        }
        private void OnMovementPreformed(InputAction.CallbackContext context)
        {
            movementInputValue = context.ReadValue<Vector2>();
        }
        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            movementInputValue = Vector2.zero;
        }
        #endregion
    }
}
