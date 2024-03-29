using HiveMind.Core.CharacterSystem.Runtime.Datas.ValueObjects;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HiveMind.Core.CharacterSystem.Runtime.Handlers.Input
{
    public abstract class InputHandler: Handler
    {
        #region ReadonlyFields
        protected readonly InputData inputData;
        protected readonly InputActionAsset inputActionAsset;
        protected readonly InputActionMap actionMap;
        protected readonly InputAction movementAction;
        protected readonly InputAction runAction;
        protected readonly InputAction mousePosAction;
        #endregion

        #region Fields
        protected Vector2 movementInputValue;
        protected Vector2 mousePosInputValue;
        protected MovementStatus movementStatus;
        #endregion

        #region Getters
        public Vector2 MovementInputValue => movementInputValue;
        public Vector2 RotationInputValue(RotationTypes rotationType)
        {
            Vector2 rotationInputValue = Vector2.zero;
            switch (rotationType)
            {
                case RotationTypes.MovementDirection:
                    rotationInputValue = movementInputValue;
                    break;
                case RotationTypes.TopDown:
                    rotationInputValue = mousePosInputValue;
                    break;
            }
            return rotationInputValue;
        }
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
            mousePosAction = actionMap.FindAction(this.inputData.InputActionNames[InputActionNameTypes.MousePosActionName]);

            SetSubscriptionStatus(movementAction, OnMovementActionStarted, OnMovementActionPerformed, OnMovementActionCanceled, true);
            SetSubscriptionStatus(runAction, OnRunActionStarted, OnRunActionPerformed, OnRunActionCanceled, true);
            SetSubscriptionStatus(mousePosAction, OnMousePosActionStarted, OnMousePosActionPerformed, OnMousePosActionCanceled, true);
        }
        #endregion

        #region Dispose
        public override void Dispose()
        {
            base.Dispose();

            SetSubscriptionStatus(movementAction, OnMovementActionStarted, OnMovementActionPerformed, OnMovementActionCanceled, false);
            SetSubscriptionStatus(runAction, OnRunActionStarted, OnRunActionPerformed, OnRunActionCanceled, false);
            SetSubscriptionStatus(mousePosAction, OnMousePosActionStarted, OnMousePosActionPerformed, OnMousePosActionCanceled, false);
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
            mousePosInputValue = Vector2.zero;
            movementStatus = MovementStatus.Walk;
        }
        protected void SetSubscriptionStatus(InputAction action, Action<InputAction.CallbackContext> onStarted, Action<InputAction.CallbackContext> onPerformed, Action<InputAction.CallbackContext> onCanceled, bool isSub)
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
        private void OnMousePosActionStarted(InputAction.CallbackContext context) => mousePosInputValue = Vector2.zero;
        private void OnMousePosActionPerformed(InputAction.CallbackContext context) => mousePosInputValue = context.ReadValue<Vector2>();
        private void OnMousePosActionCanceled(InputAction.CallbackContext context) => mousePosInputValue = Vector2.zero;
        #endregion
    }
}
