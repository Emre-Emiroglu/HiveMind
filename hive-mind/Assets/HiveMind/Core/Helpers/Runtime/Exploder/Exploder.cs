using HiveMind.Core.Helpers.Runtime.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HiveMind.Core.Helpers.Runtime.Exploder
{
    public sealed class Exploder : MonoBehaviour
    {
        #region Fields
        [Header("Explosion Settings")]
        [SerializeField] private PiecesFindingTypes findingType = PiecesFindingTypes.Physic;
        [SerializeField] private LayerMask pieceLayer;
        [SerializeField] private List<Rigidbody> pieces;
        [SerializeField] private Vector3 explosionPosOffset = Vector3.zero;
        [Range(0f, 100f)][SerializeField] private float radius = 1f;
        [Range(0f, 100f)][SerializeField] private float force = 2f;
        [Range(0f, 100f)][SerializeField] private float upwardModifier = .1f;
        [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
        [Header("Refresh Settings")]
        [SerializeField] private RefreshTypes refreshType = RefreshTypes.NonSmooth;
        [Range(0f, 10f)][SerializeField] private float smoothDuration = 1f;
        private Vector3[] _poses;
        private Quaternion[] _rots;
#if UNITY_EDITOR
        [Header("Gizmo Settings")]
        [SerializeField] private bool useGizmo;
        [SerializeField] private Color gizmoColor = Color.red;
#endif
        #endregion

        #region Core
        public void Explode()
        {
            Vector3 explosionPos = transform.position + explosionPosOffset;

            if (findingType == PiecesFindingTypes.Physic)
                FindPiecesByPhysic();

            SetPosesAndRotations();

            SetPiecesPhysicActivation(true);

            pieces.ForEach(x => x.AddExplosionForce(force, explosionPos, radius, upwardModifier, forceMode));
        }
        public void Refresh()
        {
            SetPiecesPhysicActivation(false);

            for (int i = 0; i < pieces.Count; i++)
            {
                Vector3 targetPos = _poses[i];
                Quaternion targetRot = _rots[i];

                Transform piece = pieces[i].transform;

                switch (refreshType)
                {
                    case RefreshTypes.NonSmooth:
                        piece.SetLocalPositionAndRotation(targetPos, targetRot);
                        break;
                    case RefreshTypes.Smooth:
                        StartCoroutine(SmoothRefresh(smoothDuration, piece, targetPos, targetRot));
                        break;
                }
            }
        }
        #endregion

        #region Finding
        private void FindPiecesByPhysic()
        {
            pieces = new List<Rigidbody>();

            // ReSharper disable once Unity.PreferNonAllocApi
            Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, radius, pieceLayer);

            _poses = new Vector3[colliders.Length];
            _rots = new Quaternion[colliders.Length];

            if (colliders.Length == 0)
                Debug.Log("Pieces not found. Check LayerMask or radius");
            else
                foreach (Collider t in colliders)
                    pieces.Add(t.attachedRigidbody.gameObject.GetComponent<Rigidbody>());
        }
        #endregion

        #region Sets
        private void SetPosesAndRotations()
        {
            _poses = new Vector3[pieces.Count];
            _rots = new Quaternion[pieces.Count];

            for (int i = 0; i < pieces.Count; i++)
            {
                _poses[i] = pieces[i].transform.localPosition;
                _rots[i] = pieces[i].transform.localRotation;
            }
        }
        private void SetPiecesPhysicActivation(bool isActive)
        {
            foreach (Rigidbody t in pieces)
            {
                t.isKinematic = !isActive;
                t.useGravity = isActive;
            }
        }
        #endregion

        #region Refreshing
        private IEnumerator SmoothRefresh(float duration, Transform piece, Vector3 targetPos, Quaternion targetRot)
        {
            float t = 0f;

            Vector3 oldPos = piece.transform.localPosition;
            Quaternion oldRot = piece.transform.localRotation;

            while (t < duration)
            {
                t += Time.deltaTime;

                piece.transform.localPosition = Vector3.Lerp(oldPos, targetPos, t / duration);
                piece.transform.localRotation = Quaternion.Lerp(oldRot, targetRot, t / duration);

                yield return null;
            }
        }
        #endregion

        #region Gizmo
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!useGizmo)
                return;

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireSphere(transform.position + explosionPosOffset, radius);
        }
#endif
        #endregion
    }
}
