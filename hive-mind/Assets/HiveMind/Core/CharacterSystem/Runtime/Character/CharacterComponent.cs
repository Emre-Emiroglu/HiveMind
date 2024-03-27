using HiveMind.Core.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.Core.CharacterSystem.Runtime.Handlers;
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
        #endregion

        #region Core
        private void Awake()
        {
            inputHandler = new(characterSettings.InputData);
            movementHandler = new(transform, rb, characterSettings.MovementData);
            rotationHandler = new(transform, Camera.main, characterSettings.RotationData);
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
            if (characterSettings.MovementData.MovementStyle == Enums.MovementStyles.Transform)
                movementHandler?.Movement(inputHandler.MovementInputValue);
        }
        private void FixedUpdate()
        {
            if (characterSettings.MovementData.MovementStyle == Enums.MovementStyles.Rigidbody)
                movementHandler?.Movement(inputHandler.MovementInputValue);
        }
        private void LateUpdate() => rotationHandler?.Rotation(inputHandler.RotationInputValue);
        #endregion
    }
}
