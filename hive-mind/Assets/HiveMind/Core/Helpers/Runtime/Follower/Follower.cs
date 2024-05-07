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
        [SerializeField] private FollowTypes followType;
        [SerializeField] private Space positionSpaceType;
        [SerializeField] private Space rotationSpaceType;
        [SerializeField] private LerpTypes positionLerpType;
        [SerializeField] private LerpTypes rotationLerpType;
        [Header("Target Settings")]
        [SerializeField] private Space targetPositionSpaceType;
        [SerializeField] private Space targetRotationSpaceType;
        [Header("Speed Settings")]
        [Range(0f, 100)][SerializeField] private float positionLerpSpeed = .25f;
        [Range(0f, 100)][SerializeField] private float rotationLerpSpeed = .25f;
        private Transform follower;
        private Transform target;
        private bool canFollow;
        #endregion

        #region Getters
        private (Vector3, Quaternion) GetTarget()
        {
            Vector3 pos = new Vector3();
            Quaternion rot = Quaternion.identity;

            switch (targetPositionSpaceType)
            {
                case Space.World:
                    pos = target.position;
                    break;
                case Space.Self:
                    pos = target.localPosition;
                    break;
            }

            switch (targetRotationSpaceType)
            {
                case Space.World:
                    rot = target.rotation;
                    break;
                case Space.Self:
                    rot = target.localRotation;
                    break;
            }

            return (pos, rot);
        }
        #endregion

        #region Core
        public void Initialize(Transform follower, Transform target, bool withSnap = false)
        {
            this.follower = follower;
            this.target = target;

            if (withSnap)
                SetupSnap();

            canFollow = false;
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
                        follower.position = GetTarget().Item1;
                        break;
                    case Space.Self:
                        follower.localPosition = GetTarget().Item1;
                        break;
                }
            }

            if (followType.HasFlag(FollowTypes.Rotation))
            {
                switch (rotationSpaceType)
                {
                    case Space.World:
                        follower.rotation = GetTarget().Item2;
                        break;
                    case Space.Self:
                        follower.localRotation = GetTarget().Item2;
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
                                follower.position = Vector3.Lerp(follower.position, targetPos, Time.deltaTime * positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                follower.position = targetPos;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (positionLerpType)
                        {
                            case LerpTypes.Lerp:
                                follower.localPosition = Vector3.Lerp(follower.localPosition, targetPos, Time.deltaTime * positionLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                follower.localPosition = targetPos;
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
                                follower.rotation = Quaternion.Lerp(follower.rotation, targetRot, Time.deltaTime * rotationLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                follower.rotation = targetRot;
                                break;
                        }
                        break;
                    case Space.Self:
                        switch (rotationLerpType)
                        {
                            case LerpTypes.Lerp:
                                follower.localRotation = Quaternion.Lerp(follower.localRotation, targetRot, Time.deltaTime * rotationLerpSpeed);
                                break;
                            case LerpTypes.NonLerp:
                                follower.localRotation = targetRot;
                                break;
                        }
                        break;
                }
            }
        }
        #endregion

        #region SetCanFollowStatus
        public void SetCanFollow(bool canFollow) => this.canFollow = canFollow;
        #endregion

        #region Updates
        public void ExtrenalUpdate()
        {
            if (!canFollow)
                return;

            FollowLogic();
        }
        #endregion
    }
}
