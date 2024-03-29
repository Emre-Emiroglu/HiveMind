using HiveMind.Core.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.Core.CharacterSystem.Runtime.Handlers.Input;
using HiveMind.Core.CharacterSystem.Runtime.Handlers.Movement;
using HiveMind.Core.CharacterSystem.Runtime.Handlers.Rotation;
using HiveMind.Core.CharacterSystem.Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace HiveMind.Core.CharacterSystem.Runtime.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CharacterComponent : MonoBehaviour
    {
        #region Events
        public UnityAction<bool> ChangeInputEnableStatus;
        public UnityAction<bool> ChangeMovementEnableStatus;
        public UnityAction<bool> ChangeRotationEnableStatus;
        #endregion

        #region SerializeFields
        [Header("Character Component Fields")]
        [SerializeField] private CharacterSettings characterSettings;
        [SerializeField] private Rigidbody rb;
        #endregion

        #region Fields
        private InputHandler inputHandler;
        private MovementHandler movementHandler;
        private RotationHandler rotationHandler;
        private InputTypes inputType;
        private MovementTypes movementType;
        private RotationTypes rotationType;
        #endregion

        #region Core
        private void Awake()
        {
            inputType = characterSettings.InputData.InputType;
            movementType = characterSettings.MovementData.MovementType;
            rotationType = characterSettings.RotationData.RotationType;

            switch (inputType)
            {
                case InputTypes.PC:
                    inputHandler = new PCInputHandler(characterSettings.InputData);
                    break;
                case InputTypes.Gamepad:
                    inputHandler = new GamepadInputHandler(characterSettings.InputData);
                    break;
            }

            switch (movementType)
            {
                case MovementTypes.Transform:
                    movementHandler = new TransformMovementHandler(transform, characterSettings.MovementData);
                    break;
                case MovementTypes.Rigidbody:
                    movementHandler = new RigidbodyMovementHandler(rb, characterSettings.MovementData);
                    break;
            }

            switch (rotationType)
            {
                case RotationTypes.MovementDirection:
                    rotationHandler = new MovementDirectionRotationHandler(Camera.main, transform, characterSettings.RotationData);
                    break;
                case RotationTypes.TopDown:
                    rotationHandler = new TopDownRotationHandler(Camera.main, transform, characterSettings.RotationData);
                    break;
            }
        }
        private void OnEnable()
        {
            ChangeInputEnableStatus += OnInputEnableStatusChanged;
            ChangeMovementEnableStatus += OnMovementEnableStatusChanged;
            ChangeRotationEnableStatus += OnRotationEnableStatusChanged;
        }
        private void OnDisable()
        {
            ChangeInputEnableStatus -= OnInputEnableStatusChanged;
            ChangeMovementEnableStatus -= OnMovementEnableStatusChanged;
            ChangeRotationEnableStatus -= OnRotationEnableStatusChanged;
        }
        private void OnDestroy()
        {
            inputHandler?.Dispose();
            movementHandler?.Dispose();
            rotationHandler?.Dispose();
        }
        #endregion

        #region Receivers
        private void OnInputEnableStatusChanged(bool isEnable) => inputHandler?.SetEnableStatus(isEnable);
        private void OnMovementEnableStatusChanged(bool isEnable) => movementHandler?.SetEnableStatus(isEnable);
        private void OnRotationEnableStatusChanged(bool isEnable) => rotationHandler?.SetEnableStatus(isEnable);
        #endregion

        #region Cycle
        private void Update()
        {
            if (movementType == MovementTypes.Transform)
                movementHandler?.Execute(inputHandler.MovementInputValue, inputHandler.MovementStatus);

            rotationHandler?.Execute(inputHandler.RotationInputValue(rotationType));
        }
        private void FixedUpdate()
        {
            if (movementType == MovementTypes.Rigidbody)
                movementHandler?.Execute(inputHandler.MovementInputValue, inputHandler.MovementStatus);
        }
        #endregion
    }
}
