using HiveMind.Core.Helpers.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace HiveMind.Core.Helpers.Physics
{
    public abstract class Contactlistener: MonoBehaviour
    {
        #region Fields
        [Header("Contact Listener Settings")]
        [SerializeField] protected ContactTypes contactType;
        [SerializeField] protected string[] contactableTags;
        [Header("Receivers")]
        [SerializeField] protected UnityEvent EnterCallBack;
        [SerializeField] protected UnityEvent StayCallBack;
        [SerializeField] protected UnityEvent ExitCallBack;
        #endregion

        #region Checks
        protected bool CompareCheck(string tag)
        {
            bool result;
            int tagsCount = contactableTags.Length;
            if (tagsCount == 0)
            {
                Debug.LogError("Contacable Tags Cannot Be 0!");
                result = false;
            }
            else
            {
                for (int i = 0; i < tagsCount; i++)
                {
                    bool isEqual = contactableTags[i] == tag;
                    if (isEqual)
                        break;
                }
                result = true;
            }

            return result;
        }
        #endregion

        #region Logics
        protected void ContactStatus(ContactStatusTypes contactStatusType, string tag)
        {
            bool isContain = CompareCheck(tag);
            if (isContain)
            {
                switch (contactStatusType)
                {
                    case ContactStatusTypes.Enter:
                        EnterCallBack?.Invoke();
                        break;
                    case ContactStatusTypes.Stay:
                        StayCallBack?.Invoke();
                        break;
                    case ContactStatusTypes.Exit:
                        ExitCallBack?.Invoke();
                        break;
                }
            }
            else
                Debug.Log($"{tag} tag is not in contactableTags");
        }
        #endregion
    }
}
