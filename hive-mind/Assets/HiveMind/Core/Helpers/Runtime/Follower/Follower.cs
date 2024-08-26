using HiveMind.Core.Helpers.Runtime.Enums;
using System;
using UnityEngine;

namespace HiveMind.Core.Helpers.Runtime.Follower
{
    [Serializable]
    public sealed class Follower
    {
        #region Fields
        [Header("Follower Settings")]
        [SerializeField] private FollowTypes _followType;
        [SerializeField] private Space _positionSpaceType;
        [SerializeField] private Space _rotationSpaceType;
        [SerializeField] private LerpTypes _positionLerpType;
        [SerializeField] private LerpTypes _rotationLerpType;
        [Header("Target Settings")]
        [SerializeField] private Space _targetPositionSpaceType;
        [SerializeField] private Space _targetRotationSpaceType;
        [Header("Speed Settings")]
        [Range(0f, 100)][SerializeField] private float _positionLerpSpeed = .25f;
        [Range(0f, 100)][SerializeField] private float _rotationLerpSpeed = .25f;
        private Transform _follower;
        private Transform _target;
        private bool _canFollow;
        #endregion

        #region Getters
        private (Vector3, Quaternion) GetTarget()
        {
            Vector3 pos = new();
            Quaternion rot = Quaternion.identity;

            switch (_targetPositionSpaceType)
            {
                case Space.World:
                    pos = _target.position;
                    break;
                case Space.Self:
                    pos = _target.localPosition;
                    break;
            }

            switch (_targetRotationSpaceType)
            {
                case Space.World:
                    rot = _target.rotation;
                    break;
                case Space.Self:
                    rot = _target.localRotation;
                    break;
            }

            return (pos, rot);
        }
        #endregion

        #region Core
        public void Initialize(Transform follower, Transform target, bool withSnap = false)
        {
            _follower = follower;
            _target = target;

            if (withSnap)
                SetupSnap();

            _canFollow = false;
        }
        #endregion

        #region Snapping
        private void SetupSnap()
        {
            if (_followType.HasFlag(FollowTypes.Position))
            {
                switch (_positionSpaceType)
                {
                    case Space.World:
                        _follower.position = GetTarget().Item1;
                        break;
                    case Space.Self:
                        _follower.localPosition = GetTarget().Item1;
                        break;
                }
            }

            if (_followType.HasFlag(FollowTypes.Rotation))
            {
                switch (_rotationSpaceType)
                {
                    case Space.World:
                        _follower.rotation = GetTarget().Item2;
                        break;
                    case Space.Self:
                        _follower.localRotation = GetTarget().Item2;
                        break;
                }
            }
        }
        #endregion

        #region Follows
        private void FollowLogic()
        {
            Vector3 targetPos = GetTarget().Item1;
            Quaternion targetRot = GetTarget().Item2;

            if (_followType.HasFlag(FollowTypes.Position))
            {
                switch (_positionSpaceType)
                {
                    case Space.World:
                        switch (_positionLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.position = Vector3.Lerp(_follower.position, targetPos, Time.deltaTime * _positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.position = targetPos;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (_positionLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.localPosition = Vector3.Lerp(_follower.localPosition, targetPos, Time.deltaTime * _positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.localPosition = targetPos;
                                break;
                        }
                        break;
                }
            }

            if (_followType.HasFlag(FollowTypes.Rotation))
            {
                switch (_rotationSpaceType)
                {
                    case Space.World:
                        switch (_rotationLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.rotation = Quaternion.Lerp(_follower.rotation, targetRot, Time.deltaTime * _rotationLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.rotation = targetRot;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (_rotationLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.localRotation = Quaternion.Lerp(_follower.localRotation, targetRot, Time.deltaTime * _rotationLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.localRotation = targetRot;
                                break;
                        }
                        break;
                }
            }
        }
        #endregion

        #region SetCanFollowStatus
        public void SetCanFollow(bool canFollow) => _canFollow = canFollow;
        #endregion

        #region Updates
        public void ExtrenalUpdate()
        {
            if (!_canFollow)
                return;

            FollowLogic();
        }
        #endregion
    }
}
