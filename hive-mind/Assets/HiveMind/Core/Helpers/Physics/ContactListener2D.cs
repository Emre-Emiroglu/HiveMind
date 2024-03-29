using HiveMind.Core.Helpers.Enums;
using UnityEngine;

namespace HiveMind.Core.Helpers.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class ContactListener2D : Contactlistener
    {
        #region Triggers
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Enter, collision.gameObject.tag);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Stay, collision.gameObject.tag);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Exit, collision.gameObject.tag);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Enter, collision.gameObject.tag);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Stay, collision.gameObject.tag);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Exit, collision.gameObject.tag);
        }
        #endregion
    }
}
