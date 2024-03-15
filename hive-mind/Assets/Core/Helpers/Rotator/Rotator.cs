using UnityEngine;

namespace HiveMind.Helpers.Rotator
{
    public sealed class Rotator : MonoBehaviour
    {
        #region Fields
        [Header("Rotator Settings")]
        [SerializeField] private Space space = Space.World;
        [SerializeField] private Vector3 axis = Vector3.right;
        [Range(0f, 360f)][SerializeField] private float speed = 180f;
        private bool canRotate;
        #endregion

        #region Rotate
        private void Rotate() => transform.Rotate(axis, speed * Time.deltaTime, space);
        #endregion

        #region SetCanRotateStatus
        public void SetCanRotate(bool canRotate) => this.canRotate = canRotate;
        #endregion

        #region Update
        public void ExternalUpdate()
        {
            if (!canRotate)
                return;

            Rotate();
        }
        #endregion
    }
}
