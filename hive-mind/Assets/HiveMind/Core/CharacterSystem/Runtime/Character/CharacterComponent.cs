using HiveMind.CharacterSystem.Runtime.Datas.ScriptableObjects;
using HiveMind.CharacterSystem.Runtime.Handlers;
using UnityEngine;
using UnityEngine.Events;

namespace HiveMind.CharacterSystem.Runtime.Character
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterComponent : MonoBehaviour
    {
        #region SerializeFields
        [Header("Character Component Fields")]
        [SerializeField] private CharacterSettings characterSettings;
        [SerializeField] private Rigidbody rb;
        [Header("Character Component Events")]
        [SerializeField] private UnityEvent<bool> characterControllerInputEnableStatus;
        [SerializeField] private UnityEvent<bool> characterControllerMovementEnableStatus;
        [SerializeField] private UnityEvent<bool> characterControllerRotationEnableStatus;
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
            rotationHandler = new(transform, characterSettings.RotationData);
        }
        private void OnEnable()
        {
            characterControllerInputEnableStatus.AddListener(OnCharacterControllerInputEnableStatusChanged);
            characterControllerMovementEnableStatus.AddListener(OnCharacterControllerMovementEnableStatusChanged);
            characterControllerRotationEnableStatus.AddListener(OnCharacterControllerRotationEnableStatusChanged);
        }
        private void OnDisable()
        {
            characterControllerInputEnableStatus.RemoveListener(OnCharacterControllerInputEnableStatusChanged);
            characterControllerMovementEnableStatus.RemoveListener(OnCharacterControllerMovementEnableStatusChanged);
            characterControllerRotationEnableStatus.RemoveListener(OnCharacterControllerRotationEnableStatusChanged);
        }
        private void OnDestroy()
        {
            inputHandler?.Dispose();
            movementHandler?.Dispose();
            rotationHandler?.Dispose();
        }
        #endregion

        #region Receivers
        private void OnCharacterControllerInputEnableStatusChanged(bool isEnable) => inputHandler?.SetEnableStatus(isEnable);
        private void OnCharacterControllerMovementEnableStatusChanged(bool isEnable) => movementHandler?.SetEnableStatus(isEnable);
        private void OnCharacterControllerRotationEnableStatusChanged(bool isEnable) => rotationHandler?.SetEnableStatus(isEnable);
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
