using HiveMind.Core.Helpers.Runtime.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiveMind.Core.Helpers.Runtime.Exploder
{
    public sealed class Exploder : MonoBehaviour
    {
        #region Fields
        [Header("Explosion Settings")]
        [SerializeField] private PiecesFindingTypes _findingType = PiecesFindingTypes.Physic;
        [SerializeField] private LayerMask _pieceLayer;
        [SerializeField] private List<Rigidbody> _pieces;
        [SerializeField] private Vector3 _explosionPosOffset = Vector3.zero;
        [Range(0f, 100f)][SerializeField] private float _radius = 1f;
        [Range(0f, 100f)][SerializeField] private float _force = 2f;
        [Range(0f, 100f)][SerializeField] private float _upwardModifier = .1f;
        [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;
        [Header("Refresh Settings")]
        [SerializeField] private RefreshTypes _refreshType = RefreshTypes.NonSmooth;
        [Range(0f, 10f)][SerializeField] private float _smoothDuration = 1f;
        private Vector3[] _poses;
        private Quaternion[] _rots;
        [Header("Gizmo Settings")]
        [SerializeField] private bool _useGizmo = false;
        [SerializeField] private Color _gizmoColor = Color.red;
        #endregion

        #region Core
        public void Explode()
        {
            Vector3 explosionPos = transform.position + _explosionPosOffset;

            if (_findingType == PiecesFindingTypes.Physic)
                FindPiecesByPhysic();

            SetPosesAndRotations();

            SetPiecesPhysicActivation(true);

            _pieces.ForEach(x => x.AddExplosionForce(_force, explosionPos, _radius, _upwardModifier, _forceMode));
        }
        public void Refresh()
        {
            SetPiecesPhysicActivation(false);

            for (int i = 0; i < _pieces.Count; i++)
            {
                Vector3 targetPos = _poses[i];
                Quaternion targetRot = _rots[i];

                Transform piece = _pieces[i].transform;

                switch (_refreshType)
                {
                    case RefreshTypes.NonSmooth:
                        piece.SetLocalPositionAndRotation(targetPos, targetRot);
                        break;
                    case RefreshTypes.Smooth:
                        StartCoroutine(SmoothRefresh(_smoothDuration, piece, targetPos, targetRot));
                        break;
                }
            }
        }
        #endregion

        #region Finding
        private void FindPiecesByPhysic()
        {
            _pieces = new List<Rigidbody>();

            Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, _radius, _pieceLayer);

            _poses = new Vector3[colliders.Length];
            _rots = new Quaternion[colliders.Length];

            if (colliders.Length == 0)
                Debug.Log("Pieces not found. Check LayerMask or radius");
            else
                for (int i = 0; i < colliders.Length; i++)
                    _pieces.Add(colliders[i].attachedRigidbody.gameObject.GetComponent<Rigidbody>());
        }
        #endregion

        #region Sets
        private void SetPosesAndRotations()
        {
            _poses = new Vector3[_pieces.Count];
            _rots = new Quaternion[_pieces.Count];

            for (int i = 0; i < _pieces.Count; i++)
            {
                _poses[i] = _pieces[i].transform.localPosition;
                _rots[i] = _pieces[i].transform.localRotation;
            }
        }
        private void SetPiecesPhysicActivation(bool isActive)
        {
            for (int i = 0; i < _pieces.Count; i++)
            {
                _pieces[i].isKinematic = !isActive;
                _pieces[i].useGravity = isActive;
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
        private void OnDrawGizmosSelected()
        {
            if (!_useGizmo)
                return;

            Gizmos.color = _gizmoColor;
            Gizmos.DrawWireSphere(transform.position + _explosionPosOffset, _radius);
        }
        #endregion
    }
}
