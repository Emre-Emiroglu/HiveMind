using UnityEngine;

namespace HiveMind.Core.Helpers.Runtime.Rotator
{
    public sealed class Rotator : MonoBehaviour
    {
        #region Fields
        [Header("Rotator Settings")]
        [SerializeField] private Space _space = Space.World;
        [SerializeField] private Vector3 _axis = Vector3.right;
        [Range(0f, 360f)][SerializeField] private float _speed = 180f;
        private bool _canRotate;
        #endregion

        #region Rotate
        private void Rotate() => transform.Rotate(_axis, _speed * Time.deltaTime, _space);
        #endregion

        #region SetCanRotateStatus
        public void SetCanRotate(bool canRotate) => _canRotate = canRotate;
        #endregion

        #region Update
        public void ExternalUpdate()
        {
            if (!_canRotate)
                return;

            Rotate();
        }
        #endregion
    }
}
