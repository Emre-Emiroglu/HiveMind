using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers
{
    public sealed class InputHandler: Handler
    {
        #region ReadonlyFields
        private readonly InputData inputData;
        private readonly InputActionAsset inputActionAsset;
        private readonly InputActionMap actionMap;
        private readonly InputAction movementAction;
        private readonly InputAction runAction;
        private readonly InputAction jumpAction;
        #endregion

        #region Fields
        private Vector2 movementInputValue;
        private MovementStatus movementStatus;
        #endregion

        #region Getters
        public Vector2 MovementInputValue => movementInputValue;
        public MovementStatus MovementStatus => movementStatus;
        #endregion

        #region Constructor
        public InputHandler(InputData inputData) : base()
        {
            this.inputData = inputData;
            inputActionAsset = this.inputData.InputActionAsset;

            actionMap = inputActionAsset.FindActionMap(this.inputData.InputActionNames[InputActionNameTypes.ActionMapName]);

            movementAction = actionMap.FindAction(this.inputData.InputActionNames[InputActionNameTypes.MovementActionName]);
            runAction = actionMap.FindAction(this.inputData.InputActionNames[InputActionNameTypes.RunActionName]);
            jumpAction = actionMap.FindAction(this.inputData.InputActionNames[InputActionNameTypes.JumpActionName]);

            SetSubscriptionStatus(movementAction, OnMovementActionStarted, OnMovementActionPerformed, OnMovementActionCanceled, true);
            SetSubscriptionStatus(runAction, OnRunActionStarted, OnRunActionPerformed, OnRunActionCanceled, true);
            SetSubscriptionStatus(jumpAction, OnJumpActionStarted, OnJumpActionPerformed, OnJumpActionCanceled, true);
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            base.Dispose();

            SetSubscriptionStatus(movementAction, OnMovementActionStarted, OnMovementActionPerformed, OnMovementActionCanceled, false);
            SetSubscriptionStatus(runAction, OnRunActionStarted, OnRunActionPerformed, OnRunActionCanceled, false);
            SetSubscriptionStatus(jumpAction, OnJumpActionStarted, OnJumpActionPerformed, OnJumpActionCanceled, false);
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

            movementInputValue = Vector2.zero;
            movementStatus = MovementStatus.Walk;
        }
        private void SetSubscriptionStatus(InputAction action, Action<InputAction.CallbackContext> onStarted, Action<InputAction.CallbackContext> onPerformed, Action<InputAction.CallbackContext> onCanceled, bool isSub)
        {
            if (isSub)
            {
                action.started += onStarted;
                action.performed += onPerformed;
                action.canceled += onCanceled;
            }
            else
            {
                action.started -= onStarted;
                action.performed -= onPerformed;
                action.canceled -= onCanceled;
            }
        }
        #endregion

        #region Executes
        public override void Execute() => base.Execute();
        protected override void ExecuteProcess() { }
        #endregion

        #region Receivers
        private void OnMovementActionStarted(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        private void OnMovementActionPerformed(InputAction.CallbackContext context) => movementInputValue = context.ReadValue<Vector2>();
        private void OnMovementActionCanceled(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        private void OnRunActionStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Run;
        }
        private void OnRunActionPerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Run;
        }
        private void OnRunActionCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Run)
                movementStatus = MovementStatus.Walk;
        }
        private void OnJumpActionStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Jump;
        }
        private void OnJumpActionPerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Jump;
        }
        private void OnJumpActionCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Jump)
                movementStatus = MovementStatus.Walk;
        }
        #endregion
    }
}
