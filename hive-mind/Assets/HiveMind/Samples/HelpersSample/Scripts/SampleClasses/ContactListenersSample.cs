using HiveMind.Core.Helpers.Runtime.Physics;
using UnityEngine;

namespace HiveMind.Samples.HelpersSample.SampleClasses
{
    public class ContactListenersSample : MonoBehaviour
    {
        #region Fields
        [Header("Contact Listeners Sample Fields")]
        [SerializeField] private ContactListener3D contactListener3D;
        #endregion

        #region Receivers
        public void OnEnterCallBack() => Debug.Log("Enter CallBack!");
        public void OnStayCallBack() => Debug.Log("Stay CallBack!");
        public void OnExitCallBack() => Debug.Log("Exit CallBack!");
        #endregion
    }
}
