using HiveMind.Core.Helpers.Enums;
using UnityEngine;

namespace HiveMind.Core.Helpers.Physics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class ContactListener2D : Contactlistener
    {
        #region Triggers
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Enter, collider2D.gameObject.tag, null, null, null, collider2D);
        }
        private void OnTriggerStay2D(Collider2D collider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Stay, collider2D.gameObject.tag, null, null, null, collider2D);
        }
        private void OnTriggerExit2D(Collider2D collider2D)
        {
            if (contactType == ContactTypes.Trigger)
                ContactStatus(ContactStatusTypes.Exit, collider2D.gameObject.tag, null, null, null, collider2D);
        }
        #endregion

        #region Collisions
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Enter, collision2D.gameObject.tag, null, collision2D);
        }
        private void OnCollisionStay2D(Collision2D collision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Stay, collision2D.gameObject.tag, null, collision2D);
        }
        private void OnCollisionExit2D(Collision2D collision2D)
        {
            if (contactType == ContactTypes.Collision)
                ContactStatus(ContactStatusTypes.Exit, collision2D.gameObject.tag, null, collision2D);
        }
        #endregion
    }
}
