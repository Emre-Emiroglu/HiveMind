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
        private readonly InputAction[] actions;
        private readonly InputAction proneAction;
        private readonly InputAction crouchAction;
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

            actionMap = inputActionAsset.FindActionMap(this.inputData.Names[InputNameTypes.ActionMapName]);

            proneAction = actionMap.FindAction(this.inputData.Names[InputNameTypes.ProneActionName]);
            crouchAction = actionMap.FindAction(this.inputData.Names[InputNameTypes.CrouchActionName]);
            movementAction = actionMap.FindAction(this.inputData.Names[InputNameTypes.MovementActionName]);
            runAction = actionMap.FindAction(this.inputData.Names[InputNameTypes.RunActionName]);
            jumpAction = actionMap.FindAction(this.inputData.Names[InputNameTypes.JumpActionName]);

            SetSubscriptionStatus(proneAction, OnProneStarted, OnPronePerformed, OnProneCanceled, true);
            SetSubscriptionStatus(crouchAction, OnCrouchStarted, OnCrouchPerformed, OnCrouchCanceled, true);
            SetSubscriptionStatus(movementAction, OnMovementStarted, OnMovementPerformed, OnMovementCanceled, true);
            SetSubscriptionStatus(runAction, OnRunStarted, OnRunPerformed, OnRunCanceled, true);
            SetSubscriptionStatus(jumpAction, OnJumpStarted, OnJumpPerformed, OnJumpCanceled, true);
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            base.Dispose();

            SetSubscriptionStatus(proneAction, OnProneStarted, OnPronePerformed, OnProneCanceled, false);
            SetSubscriptionStatus(crouchAction, OnCrouchStarted, OnCrouchPerformed, OnCrouchCanceled, false);
            SetSubscriptionStatus(movementAction, OnMovementStarted, OnMovementPerformed, OnMovementCanceled, false);
            SetSubscriptionStatus(runAction, OnRunStarted, OnRunPerformed, OnRunCanceled, false);
            SetSubscriptionStatus(jumpAction, OnJumpStarted, OnJumpPerformed, OnJumpCanceled, false);
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
        private void OnProneStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Prone;
        }
        private void OnPronePerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Prone;
        }
        private void OnProneCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Prone)
                movementStatus = MovementStatus.Walk;
        }
        private void OnCrouchStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Crouch;
        }
        private void OnCrouchPerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Crouch;
        }
        private void OnCrouchCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Crouch)
                movementStatus = MovementStatus.Walk;
        }
        private void OnMovementStarted(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        private void OnMovementPerformed(InputAction.CallbackContext context) => movementInputValue = context.ReadValue<Vector2>();
        private void OnMovementCanceled(InputAction.CallbackContext context) => movementInputValue = Vector2.zero;
        private void OnRunStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Run;
        }
        private void OnRunPerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Run;
        }
        private void OnRunCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Run)
                movementStatus = MovementStatus.Walk;
        }
        private void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Jump;
        }
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Walk)
                movementStatus = MovementStatus.Jump;
        }
        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            if (movementStatus == MovementStatus.Jump)
                movementStatus = MovementStatus.Walk;
        }
        #endregion
    }
}
