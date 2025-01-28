using CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Physics
{
    public sealed class PhysicSample : MonoBehaviour
    {
        #region Fields
        [Header("Physic Sample Fields")]
        [SerializeField] private ContactListener3D contactListener;
        #endregion

        #region Core
        private void Awake()
        {
            contactListener.EnterCallBack += OnEnterCallBack;
            contactListener.StayCallBack += OnStayCallBack;
            contactListener.ExitCallBack += OnExitCallBack;
        }
        private void OnDestroy()
        {
            contactListener.EnterCallBack -= OnEnterCallBack;
            contactListener.StayCallBack -= OnStayCallBack;
            contactListener.ExitCallBack -= OnExitCallBack;
        }
        #endregion

        #region Receivers
        private void OnEnterCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Enter call callback");
        private void OnStayCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Stay call callback");
        private void OnExitCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Exit call callback");
        #endregion
    }
}