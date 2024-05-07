using HiveMind.Core.Helpers.Runtime.Follower;
using UnityEngine;

namespace HiveMind.Samples.HelpersSample.SampleClasses
{
    public class FollowerSample : MonoBehaviour
    {
        #region Fields
        [Header("Follower Sample Fields")]
        [SerializeField] private Follower follower;
        [SerializeField] private Transform followerTransform;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private bool withSnap = true;
        #endregion

        #region Core
        [ContextMenu("Initialize Follower")]
        public void Initialize() => follower?.Initialize(followerTransform, targetTransform, withSnap);
        #endregion

        #region SetCanFollowStatus
        [ContextMenu("Can Follow")]
        public void CanFollow() => follower?.SetCanFollow(true);
        [ContextMenu("Cant Follow")]
        public void CantFollow() => follower?.SetCanFollow(false);
        #endregion

        #region Updates
        private void Update() => follower?.ExtrenalUpdate();
        #endregion
    }
}
