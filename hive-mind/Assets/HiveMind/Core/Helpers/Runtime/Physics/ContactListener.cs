using HiveMind.Core.Helpers.Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace HiveMind.Core.Helpers.Runtime.Physics
{
    public abstract class Contactlistener: MonoBehaviour
    {
        #region Fields
        [Header("Contact Listener Settings")]
        [SerializeField] protected ContactTypes contactType;
        [SerializeField] protected string[] contactableTags;
        [Header("Receivers")]
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> enterCallBack;
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> stayCallBack;
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> exitCallBack;
        #endregion

        #region Checks
        private bool CompareCheck(string tagName)
        {
            bool result;
            int tagsCount = contactableTags.Length;
            if (tagsCount == 0)
            {
                Debug.LogError("Contactable Tags Cannot Be 0!");
                result = false;
            }
            else
            {
                for (int i = 0; i < tagsCount; i++)
                {
                    bool isEqual = contactableTags[i] == tagName;
                    if (isEqual)
                        break;
                }
                result = true;
            }

            return result;
        }
        #endregion

        #region Logics
        protected void ContactStatus(ContactStatusTypes contactStatusType, string tagName, Collision contactCollision = null, Collision2D contactCollision2D = null, Collider contactCollider = null, Collider2D contactCollider2D = null)
        {
            bool isContain = CompareCheck(tagName);
            if (isContain)
            {
                switch (contactStatusType)
                {
                    case ContactStatusTypes.Enter:
                        enterCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                    case ContactStatusTypes.Stay:
                        stayCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                    case ContactStatusTypes.Exit:
                        exitCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                }
            }
            else
                Debug.Log($"{tag} tag is not in contactableTags");
        }
        #endregion
    }
}
