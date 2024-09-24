using HiveMind.Core.Helpers.Runtime.Enums;
using UnityEngine;

namespace HiveMind.Core.Helpers.Runtime.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class ContactListener2D : Contactlistener
    {
        #region Triggers
        private void OnTriggerEnter2D(Collider2D contactCollider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Enter, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
        }
        private void OnTriggerStay2D(Collider2D contactCollider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Stay, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
        }
        private void OnTriggerExit2D(Collider2D contactCollider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Exit, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D contactCollision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Enter, contactCollision2D.gameObject.tag, null, contactCollision2D);
        }
        private void OnCollisionStay2D(Collision2D contactCollision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Stay, contactCollision2D.gameObject.tag, null, contactCollision2D);
        }
        private void OnCollisionExit2D(Collision2D contactCollision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Exit, contactCollision2D.gameObject.tag, null, contactCollision2D);
        }
        #endregion
    }
}
