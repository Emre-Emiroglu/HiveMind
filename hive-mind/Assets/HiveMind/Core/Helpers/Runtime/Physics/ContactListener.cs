using HiveMind.Core.Helpers.Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace HiveMind.Core.Helpers.Runtime.Physics
{
    public abstract class Contactlistener: MonoBehaviour
    {
        #region Fields
        [Header("Contact Listener Settings")]
        [SerializeField] protected ContactTypes _contactType;
        [SerializeField] protected string[] _contactableTags;
        [Header("Receivers")]
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> _enterCallBack;
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> _stayCallBack;
        [SerializeField] protected UnityEvent<Collision, Collision2D, Collider, Collider2D> _exitCallBack;
        #endregion

        #region Checks
        protected bool CompareCheck(string tag)
        {
            bool result;
            int tagsCount = _contactableTags.Length;
            if (tagsCount == 0)
            {
                Debug.LogError("Contacable Tags Cannot Be 0!");
                result = false;
            }
            else
            {
                for (int i = 0; i < tagsCount; i++)
                {
                    bool isEqual = _contactableTags[i] == tag;
                    if (isEqual)
                        break;
                }
                result = true;
            }

            return result;
        }
        #endregion

        #region Logics
        protected void ContactStatus(ContactStatusTypes contactStatusType, string tag, Collision collision = null, Collision2D collision2D = null, Collider collider = null, Collider2D collider2D = null)
        {
            bool isContain = CompareCheck(tag);
            if (isContain)
            {
                switch (contactStatusType)
                {
                    case ContactStatusTypes.Enter:
                        _enterCallBack?.Invoke(collision, collision2D, collider, collider2D);
                        break;
                    case ContactStatusTypes.Stay:
                        _stayCallBack?.Invoke(collision, collision2D, collider, collider2D);
                        break;
                    case ContactStatusTypes.Exit:
                        _exitCallBack?.Invoke(collision, collision2D, collider, collider2D);
                        break;
                }
            }
            else
                Debug.Log($"{tag} tag is not in contactableTags");
        }
        #endregion
    }
}
