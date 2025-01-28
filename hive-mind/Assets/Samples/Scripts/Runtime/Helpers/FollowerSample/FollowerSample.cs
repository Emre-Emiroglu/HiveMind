using CodeCatGames.HiveMind.Core.Runtime.Helpers.Follower;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.FollowerSample
{
    public class FollowerSample : MonoBehaviour
    {
        #region Fields
        [Header("Follower Sample Fields")]
        [SerializeField] private Follower follower;
        [SerializeField] private Transform followerObject;
        [SerializeField] private Transform targetObject;
        [SerializeField] private bool withSnap;
        #endregion

        #region Core
        private void Awake() => follower?.Initialize(followerObject, targetObject, withSnap);
        #endregion

        #region Executes
        [ContextMenu("Can Follow")]
        public void CanFollow() => follower?.SetCanFollow(true);
        [ContextMenu("Cant Follow")]
        public void CantFollow() => follower?.SetCanFollow(false);
        #endregion

        #region Update
        private void Update() => follower?.ExternalUpdate();
        #endregion
    }
}