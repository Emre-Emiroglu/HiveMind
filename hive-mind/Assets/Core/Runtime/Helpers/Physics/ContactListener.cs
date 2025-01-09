using System;
using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
using UnityEngine;

namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics
{
    public abstract class Contactlistener: MonoBehaviour
    {
        #region Actions
        protected Action<Collision, Collision2D, Collider, Collider2D> EnterCallBack;
        protected Action<Collision, Collision2D, Collider, Collider2D> StayCallBack;
        protected Action<Collision, Collision2D, Collider, Collider2D> ExitCallBack;
        #endregion
        
        #region Fields
        [Header("Contact Listener Settings")]
        [SerializeField] protected ContactTypes contactType;
        [SerializeField] protected string[] contactableTags;
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
                        EnterCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                    case ContactStatusTypes.Stay:
                        StayCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                    case ContactStatusTypes.Exit:
                        ExitCallBack?.Invoke(contactCollision, contactCollision2D, contactCollider, contactCollider2D);
                        break;
                }
            }
            else
                Debug.Log($"{tag} tag is not in contactableTags");
        }
        #endregion
    }
}
