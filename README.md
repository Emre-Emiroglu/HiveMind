# Hive Mind
Hive Mind, along with its components, classes and samples, includes Zenject for dependency injection, UniTask for async operations, Prime Tween for tween, Easy Save 3 for data saving and loading operations, Feel/Nice Vibrations for haptics and Sirenix/Odin Inspector for editor. It is a unity project that makes game development processes streamline, scalable, maintainable and modular.

## Features
* Core: Core of Hive Mind. Contains components and classes to be used in game development processes.
* Samples: Contains examples demonstrating how to use the core structure of Hive Mind.
* Zenject: Zenject is a lightweight highly performant dependency injection framework built specifically to target Unity 3D.
* UniTask: Provides an efficient allocation free async/await integration for Unity.
* Prime Tween: High-performance, allocation-free tween library for Unity.
* Easy Save 3: Easy Save lets you save almost anything with ease across platforms, along with features such as encryption, compression, save slots, cloud storage, spreadsheets, backups, and much more.
* Feel/Nice Vibrations: Nice Vibrations is a simple yet powerful solution to add vibrations, rumble and haptic feedbacks to your games.
* Sirenix/Odin Inspector: Odin puts your Unity workflow on steroids, making it easy to build powerful and advanced user-friendly editors for you and your entire team.

## Getting Started
Clone the repository
```bash
  git clone https://github.com/Emre-Emiroglu/HiveMind.git
```
This project is developed using Unity version 2022.3.37f1.

## Core
* Helpers: Contains components and classes that are useful and frequently needed in game development processes.
  * Countdown: This class manages the countdown process, allowing time display in a specific format, adding time, pausing, and triggering a callback when the countdown ends.
    ```csharp
    using System;
    using CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums;
    using CodeCatGames.HiveMind.Core.Runtime.Utilities.TextFormatter;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Countdown
    {
        public sealed class Countdown
        {
            #region Fields
            private TimeFormattingTypes _timeFormattingType;
            private bool _showMilliSeconds;
            private double _countdownInternal;
            private bool _pause;
            #endregion
    
            #region Getters
            public double CountDownInternal => _countdownInternal;
            public string GetFormattedTime() => TextFormatter.FormatTime(_countdownInternal, _timeFormattingType, _showMilliSeconds);
            public bool IsPause => _pause;
            #endregion
    
            #region Core
            public void Setup(TimeFormattingTypes timeFormattingType, bool showMilliSeconds, double countDownTime)
            {
                _timeFormattingType = timeFormattingType;
                _showMilliSeconds = showMilliSeconds;
                _countdownInternal = countDownTime;
                _pause = false;
            }
            #endregion
    
            #region SetStatus
            public void SetPause(bool isPause) => _pause = isPause;
            public void AddSeconds(int seconds) => _countdownInternal += seconds;
            #endregion
    
            #region Update
            public void ExternalUpdate(Action countDownEnded = null)
            {
                if (_pause)
                    return;
    
                if (_countdownInternal > 0)
                {
                    _countdownInternal -= Time.deltaTime;
                    
                    if (_countdownInternal < 0)
                    {
                        _countdownInternal = 0;
                        
                        countDownEnded?.Invoke();
                    }
                }
            }
            #endregion
        }
    }
    ```
  * Exploder: This component manages an explosion effect by detecting physics objects within a specific area, applying an explosion force to scatter them, and optionally restoring them to their original positions.
    ```csharp
    using System.Collections;
    using System.Collections.Generic;
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Exploder
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
    
                Collider[] results = new Collider[] { };
                UnityEngine.Physics.OverlapSphereNonAlloc(transform.position, radius, results, pieceLayer);
    
                _poses = new Vector3[results.Length];
                _rots = new Quaternion[results.Length];
    
                if (results.Length == 0)
                    Debug.Log("Pieces not found. Check LayerMask or radius");
                else
                    foreach (Collider t in results)
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
    ```
  * Follower: This component manages a tracking system that allows an object to follow a designated target in various position and rotation spaces, offering different smoothing (lerp) options.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Follower
    {
        public sealed class Follower : MonoBehaviour
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
            public void ExternalUpdate()
            {
                if (!_canFollow)
                    return;
    
                FollowLogic();
            }
            #endregion
        }
    }
    ```
  * Physics: These components detects and handles physical collisions and contacts, managing enter, stay, and exit events for specified objects.
    ```csharp
    using System;
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics
    {
        public abstract class Contactlistener: MonoBehaviour
        {
            #region Actions
            public Action<Collision, Collision2D, Collider, Collider2D> EnterCallBack;
            public Action<Collision, Collision2D, Collider, Collider2D> StayCallBack;
            public Action<Collision, Collision2D, Collider, Collider2D> ExitCallBack;
            #endregion
            
            #region Fields
            [Header("Contact Listener Settings")]
            [SerializeField] protected ContactTypes contactType;
            [SerializeField] protected string[] contactableTags;
            #endregion
    
            #region Checks
            private bool CompareCheck(string tagName)
            {
                int tagsCount = contactableTags.Length;
                
                if (tagsCount == 0)
                    Debug.LogError("Contactable Tags Cannot Be 0!");
                else
                {
                    for (int i = 0; i < tagsCount; i++)
                    {
                        bool isEqual = contactableTags[i] == tagName;
                        if (isEqual)
                            return true;
                    }
                }
    
                return false;
            }
            #endregion
    
            #region Logics
            protected void ContactStatus(ContactStatusTypes contactStatusType, string tagName,
                Collision contactCollision = null, Collision2D contactCollision2D = null, Collider contactCollider = null,
                Collider2D contactCollider2D = null)
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
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics
    {
        [RequireComponent(typeof(Rigidbody2D))]
        public sealed class ContactListener2D : Contactlistener
        {
            #region Triggers
            private void OnTriggerEnter2D(Collider2D contactCollider2D)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Enter, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
            }
            private void OnTriggerStay2D(Collider2D contactCollider2D)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Stay, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
            }
            private void OnTriggerExit2D(Collider2D contactCollider2D)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Exit, contactCollider2D.gameObject.tag, null, null, null, contactCollider2D);
            }
            #endregion
    
            #region Collisions
            private void OnCollisionEnter2D(Collision2D contactCollision2D)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Enter, contactCollision2D.gameObject.tag, null, contactCollision2D);
            }
            private void OnCollisionStay2D(Collision2D contactCollision2D)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Stay, contactCollision2D.gameObject.tag, null, contactCollision2D);
            }
            private void OnCollisionExit2D(Collision2D contactCollision2D)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Exit, contactCollision2D.gameObject.tag, null, contactCollision2D);
            }
            #endregion
        }
    }
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics
    {
        [RequireComponent(typeof(Rigidbody))]
        public sealed class ContactListener3D : Contactlistener
        {
            #region Triggers
            private void OnTriggerEnter(Collider contactCollider)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Enter, contactCollider.gameObject.tag, null, null, contactCollider);
            }
            private void OnTriggerStay(Collider contactCollider)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Stay, contactCollider.gameObject.tag, null, null, contactCollider);
            }
            private void OnTriggerExit(Collider contactCollider)
            {
                if (contactType == ContactTypes.Trigger)
                    ContactStatus(ContactStatusTypes.Exit, contactCollider.gameObject.tag, null, null, contactCollider);
            }
            #endregion
    
            #region Collisions
            private void OnCollisionEnter(Collision contactCollision)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Enter, contactCollision.gameObject.tag, contactCollision);
            }
            private void OnCollisionStay(Collision contactCollision)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Stay, contactCollision.gameObject.tag, contactCollision);
            }
            private void OnCollisionExit(Collision contactCollision)
            {
                if (contactType == ContactTypes.Collision)
                    ContactStatus(ContactStatusTypes.Exit, contactCollision.gameObject.tag, contactCollision);
            }
            #endregion
        }
    }
    ```
  * Rotator: This component enables continuous rotational movement by rotating objects along a specified axis at a defined speed.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Rotator
    {
        public sealed class Rotator : MonoBehaviour
        {
            #region Fields
            [Header("Rotator Settings")]
            [SerializeField] private Space space = Space.World;
            [SerializeField] private Vector3 axis = Vector3.right;
            [Range(0f, 360f)][SerializeField] private float speed = 180f;
            private bool _canRotate;
            #endregion
    
            #region Rotate
            private void Rotate() => transform.Rotate(axis, speed * Time.deltaTime, space);
            #endregion
    
            #region SetCanRotateStatus
            public void SetCanRotate(bool canRotate) => _canRotate = canRotate;
            #endregion
    
            #region Update
            public void ExternalUpdate()
            {
                if (!_canRotate)
                    return;
    
                Rotate();
            }
            #endregion
        }
    }
    ```
  * SlowMotion: This class manages the activation and deactivation of a slow-motion effect by adjusting the game's timescale.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.SlowMotion
    {
        public sealed class SlowMotion 
        {
            #region Constants
            private const float TimeStep = .02f;
            private const float TimeScale = 1f;
            #endregion
    
            #region Fields
            private float _factor = .25f;
            #endregion
    
            #region Core
            public void Setup(float factor) => _factor = factor;
            #endregion
    
            #region SetActivation
            public void Activate()
            {
                Time.timeScale = _factor;
                Time.fixedDeltaTime = _factor * TimeStep;
            }
            public void DeActivate()
            {
                Time.timeScale = TimeScale;
                Time.fixedDeltaTime = TimeStep;
            }
            #endregion
        }
    }
    ```
* MVC: Contains components and classes that enable the use of the MVC architectural pattern together with Zenject.
  * Model: This class contains and encapsulates data. Each model class must contain and encapsulate a ScriptableObject component. The Model class and the ScriptableObject component have a one-to-one relationship. The ScriptableObject component contains and encapsulates non-persistent in-game data.
    ```csharp
    ```
  * View & Mediator: The View component contains only the components and variables related to the visual appearance of the game object without any operations. The Mediator class provides communication between the View component and the rest of the system. Each mediator class must contain and encapsulate a view component. The Mediator class and the View component have a one-to-one relationship.
    ```csharp
    ```
  * Command: If the Command class has a signal structure attached to it and this signal is fired, it runs the Execute method. Thanks to the Command class, MVC architectural pattern and signal-oriented development are applied together.
    ```csharp
    ```
  * ViewMediatorInstaller: It is a Zenject Installer specially developed to ensure that the one-to-one relationship between the Mediator class and the View component can be implemented in cases where there is more than one View component in the game scene.
    ```csharp
    ```
* ProDebug: Contains classes that provides writing log messages in color and different formats.
  * Colorize: This class is a utility for coloring text in Unity's Debug.Log messages and other text outputs, supporting both RGB and HEX color formats.
    ```csharp
    ```
  * TextFormat: This class is a utility for formatting text in Unity's Debug.Log messages and other text outputs, allowing text to be bold or italic.
    ```csharp
    ```
* Utilities: Contains utility classes that are useful and frequently needed in game development processes.
  * Extensions: This class adds extension methods to the Transform component in Unity, enabling axis-based position setting, looking at a target along a specific axis, gradual look direction adjustment, hierarchical object searching, and isometric transformation.
    ```csharp
    ```
  * FPSDisplay: This component is a Unity component that displays the FPS (frames per second) value on the screen and updates it at set intervals for performance monitoring.
    ```csharp
    ```
  * TextFormatter: This class is a utility component that manages number and time formatting, providing methods to convert numbers into readable formats and support various time display formats.
    ```csharp
    ```
  * TimeCalculator: This class provides various time calculation and formatting utility methods to manage local and server-based date/time operations, including computing time differences, adding time, and tracking elapsed time.
    ```csharp
    ```
  * Utilities: This class provides utility functions for Unity, including tag creation, adjusting CanvasGroup alpha, converting world positions to UI screen positions, shuffling lists, bubble sorting, and performing isometric transformations.
    ```csharp
    ```

## Samples
* Helpers: Contains components that demonstrating how to use Helpers components and classes.
  * CountdownSample: This component demonstrating how to use the Countdown class.
    ```csharp
    ```
  * ExploderSample: This component demonstrating how to use the Exploder component.
    ```csharp
    ```
  * FollowerSample: This component demonstrating how to use the Follower component.
    ```csharp
    ```
  * PhysicSample: This component demonstrating how to use the Physics components.
    ```csharp
    ```
  * RotatorSample: This component demonstrating how to use the Rotator component.
    ```csharp
    ```
  * SlowMotionSample: This component demonstrating how to use the SlowMotion class.
    ```csharp
    ```
* MVC: Contains components and classes that demonstrating how to use MVC components and classes.
  * Models & Data: These components and classes demonstrating how to use the Model class.
    ```csharp
    ```
  * Views & Mediators: These components and classes demonstrating how to use the View component and Mediator class.
    ```csharp
    ```
  * Commands & Signals: These classes and structs demonstrating how to use the Command class and performs the signal-oriented development.
    ```csharp
    ```
  * Installers: This component demonstrating how to use the ViewMediatorInstaller class and performs the binding operations for Zenject.
    ```csharp
    ```
* ProDebug: Contains component that demonstrating how to use ProDebug classes.
  * ProDebugSample: This component demonstrating how to use the Colorize and TextFormat classes.
    ```csharp
    ```
* SampleGame: It uses HiveMind's components and classes, as well as its own components and classes. It is a template that shows how to develop a game with HiveMind.

## Acknowledgments
Special thanks to the developers of Zenject, UniTask, Prime Tween, Easy Save 3, Feel/Nice Vibrations, Sirenix/Odin Inspector and the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.

## Dependencies
* Zenject
* UniTask
* Prime Tween
* Easy Save 3
* Feel/Nice Vibrations
* Sirenix/Odin Inspector
