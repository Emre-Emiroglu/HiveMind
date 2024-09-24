using HiveMind.Core.Helpers.Runtime.Enums;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HiveMind.Core.Helpers.Runtime.Follower
{
    [Serializable]
    public sealed class Follower
    {
        #region Fields
        [Header("Follower Settings")]
        [SerializeField] private FollowTypes followType;
        [SerializeField] private Space positionSpaceType;
        [SerializeField] private Space rotationSpaceType;
        [SerializeField] private LerpTypes positionLerpType;
        [SerializeField] private LerpTypes rotationLerpType;
        [Header("Target Settings")]
        [SerializeField] private Space targetPositionSpaceType;
        [FormerlySerializedAs("_targetRotationSpaceType")] [SerializeField] private Space targetRotationSpaceType;
        [Header("Speed Settings")]
        [Range(0f, 100)][SerializeField] private float positionLerpSpeed = .25f;
        [Range(0f, 100)][SerializeField] private float rotationLerpSpeed = .25f;
        private Transform _follower;
        private Transform _target;
        private bool _canFollow;
        #endregion

        #region Getters
        private (Vector3, Quaternion) GetTarget()
        {
            Vector3 pos = new();
            Quaternion rot = Quaternion.identity;

            switch (targetPositionSpaceType)
            {
                case Space.World:
                    pos = _target.position;
                    break;
                case Space.Self:
                    pos = _target.localPosition;
                    break;
            }

            switch (targetRotationSpaceType)
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
            if (followType.HasFlag(FollowTypes.Position))
            {
                switch (positionSpaceType)
                {
                    case Space.World:
                        _follower.position = GetTarget().Item1;
                        break;
                    case Space.Self:
                        _follower.localPosition = GetTarget().Item1;
                        break;
                }
            }

            if (followType.HasFlag(FollowTypes.Rotation))
            {
                switch (rotationSpaceType)
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

            if (followType.HasFlag(FollowTypes.Position))
            {
                switch (positionSpaceType)
                {
                    case Space.World:
                        switch (positionLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.position = Vector3.Lerp(_follower.position, targetPos, Time.deltaTime * positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.position = targetPos;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (positionLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.localPosition = Vector3.Lerp(_follower.localPosition, targetPos, Time.deltaTime * positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.localPosition = targetPos;
                                break;
                        }
                        break;
                }
            }

            if (followType.HasFlag(FollowTypes.Rotation))
            {
                switch (rotationSpaceType)
                {
                    case Space.World:
                        switch (rotationLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.rotation = Quaternion.Lerp(_follower.rotation, targetRot, Time.deltaTime * rotationLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                _follower.rotation = targetRot;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (rotationLerpType)
                        {
                            case LerpTypes.Lerp:
                                _follower.localRotation = Quaternion.Lerp(_follower.localRotation, targetRot, Time.deltaTime * rotationLerpSpeed);
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
