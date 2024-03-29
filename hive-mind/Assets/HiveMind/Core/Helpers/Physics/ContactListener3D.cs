using HiveMind.Core.Helpers.Enums;
using UnityEngine;

namespace HiveMind.Core.Helpers.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ContactListener3D : Contactlistener
    {
        #region Triggers
        private void OnTriggerEnter(Collider collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Enter, collision.gameObject.tag);
        }
        private void OnTriggerStay(Collider collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Stay, collision.gameObject.tag);
        }
        private void OnTriggerExit(Collider collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Exit, collision.gameObject.tag);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Enter, collision.gameObject.tag);
        }
        private void OnCollisionStay(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Stay, collision.gameObject.tag);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Exit, collision.gameObject.tag);
        }
        #endregion
    }
}
