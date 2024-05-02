using HiveMind.Core.Helpers.Enums;
using UnityEngine;

namespace HiveMind.Core.Helpers.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class ContactListener3D : Contactlistener
    {
        #region Triggers
        private void OnTriggerEnter(Collider collider)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Enter, collider.gameObject.tag, null, null, collider);
        }
        private void OnTriggerStay(Collider collider)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Stay, collider.gameObject.tag, null, null, collider);
        }
        private void OnTriggerExit(Collider collider)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Exit, collider.gameObject.tag, null, null, collider);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Enter, collision.gameObject.tag, collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Stay, collision.gameObject.tag, collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Exit, collision.gameObject.tag, collision);
        }
        #endregion
    }
}
