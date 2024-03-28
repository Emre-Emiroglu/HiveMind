using HiveMind.Core.CharacterSystem.Runtime.Character;
using UnityEngine;

namespace HiveMind.Samples.CharacterSystemSample.SampleClasses
{
    public class SampleCharacterController : MonoBehaviour
    {
        #region Fields
        [Header("Sample Character Controller Fields")]
        [SerializeField] private CharacterComponent characterComponent;
        [SerializeField] private bool enableAllHandlersOnStart = true;
        #endregion

        #region Core
        private void Start()
        {
            if (!enableAllHandlersOnStart)
                return;

            EnableInput();
            EnableMovement();
            EnableRotation();
        }
        #endregion

        #region Triggers
        [ContextMenu("EnableInput")]
        public void EnableInput() => characterComponent.ChangeInputEnableStatus?.Invoke(true);
        [ContextMenu("DisableInput")]
        public void DisableInput() => characterComponent.ChangeInputEnableStatus?.Invoke(false);
        [ContextMenu("EnableMovement")]
        public void EnableMovement() => characterComponent.ChangeMovementEnableStatus?.Invoke(true);
        [ContextMenu("DisableMovement")]
        public void DisableMovement() => characterComponent.ChangeMovementEnableStatus?.Invoke(false);
        [ContextMenu("EnableRotation")]
        public void EnableRotation() => characterComponent.ChangeRotationEnableStatus?.Invoke(true);
        [ContextMenu("DisableRotation")]
        public void DisableRotation() => characterComponent.ChangeRotationEnableStatus?.Invoke(false);
        #endregion
    }
}
