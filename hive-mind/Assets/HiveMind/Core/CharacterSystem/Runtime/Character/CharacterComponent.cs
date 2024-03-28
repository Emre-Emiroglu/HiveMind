using HiveMind.Core.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.Core.CharacterSystem.Runtime.Handlers;
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
        #endregion

        #region SerializeFields
        [Header("Character Component Fields")]
        [SerializeField] private CharacterSettings characterSettings;
        [SerializeField] private Rigidbody rb;
        #endregion

        #region Fields
        private InputHandler inputHandler;
        private MovementHandler movementHandler;
        #endregion

        #region Core
        private void Awake()
        {
            inputHandler = new(characterSettings.InputData);
            switch (characterSettings.MovementData.MovementStyle)
            {
                case MovementStyles.Transform:
                    movementHandler = new TransformMovementHandler(transform, characterSettings.MovementData);
                    break;
                case MovementStyles.Rigidbody:
                    movementHandler = new RigidbodyMovementHandler(rb, characterSettings.MovementData);
                    break;
            }
        }
        private void OnEnable()
        {
            ChangeInputEnableStatus += OnInputEnableStatusChanged;
            ChangeMovementEnableStatus += OnMovementEnableStatusChanged;
        }
        private void OnDisable()
        {
            ChangeInputEnableStatus -= OnInputEnableStatusChanged;
            ChangeMovementEnableStatus -= OnMovementEnableStatusChanged;
        }
        private void OnDestroy()
        {
            inputHandler?.Dispose();
            movementHandler?.Dispose();
        }
        #endregion

        #region Receivers
        private void OnInputEnableStatusChanged(bool isEnable) => inputHandler?.SetEnableStatus(isEnable);
        private void OnMovementEnableStatusChanged(bool isEnable) => movementHandler?.SetEnableStatus(isEnable);
        #endregion

        #region Cycle
        private void Update()
        {
            if (characterSettings.MovementData.MovementStyle == MovementStyles.Transform)
                movementHandler?.Execute(inputHandler.MovementInputValue);
        }
        private void FixedUpdate()
        {
            if (characterSettings.MovementData.MovementStyle == MovementStyles.Rigidbody)
                movementHandler?.Execute(inputHandler.MovementInputValue);
        }
        #endregion
    }
}
