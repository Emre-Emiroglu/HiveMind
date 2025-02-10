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
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums
    {
        public enum PiecesFindingTypes
        {
            Inspector,
            Physic
        }
    
        public enum RefreshTypes
        {
            NonSmooth,
            Smooth
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
    ```csharp
    using System;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums
    {
        [Flags]
        public enum FollowTypes
        {
            None = 0,
            Position = 1,
            Rotation = 2,
            Everything = 3,
        }
    
        public enum LerpTypes
        {
            Lerp,
            NonLerp
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
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.Helpers.Enums
    {
        public enum ContactTypes
        {
            Trigger,
            Collision
        }
    
        public enum ContactStatusTypes
        {
            Enter,
            Stay,
            Exit
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
    using UnityEngine;
    using Zenject;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.MVC.Model
    {
        public abstract class Model<TModelSettings> where TModelSettings : ScriptableObject
        {
            #region Fields
            private readonly TModelSettings _settings;
            #endregion
    
            #region Getters
            public TModelSettings GetSettings => _settings;
            #endregion
    
            #region Constructor
            public Model(string resourcePath)
            {
                if (resourcePath == string.Empty)
                {
                    Debug.Log("Resource path can not be null for getting model settings!");
                    return;
                }
                
                _settings = Resources.Load<TModelSettings>(resourcePath);
            }
            #endregion
    
            #region PostConstruct
            [Inject] public abstract void PostConstruct();
            #endregion
        }
    }
    ```
  * View & Mediator: The View component contains only the components and variables related to the visual appearance of the game object without any operations. The Mediator class provides communication between the View component and the rest of the system. Each mediator class must contain and encapsulate a view component. The Mediator class and the View component have a one-to-one relationship.
    ```csharp
    using Sirenix.OdinInspector;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.MVC.View
    {
        public abstract class View : SerializedMonoBehaviour
        {
        }
    }
    ```
    ```csharp
    using System;
    using Zenject;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.MVC.View
    {
        public abstract class Mediator<TView> : IInitializable, IDisposable
            where TView : View
        {
            #region Fields
            private readonly TView _view;
            #endregion
    
            #region Getters
            public TView GetView => _view;
            #endregion
    
            #region Constructor
            public Mediator(TView view) => _view = view;
            #endregion
    
            #region PostConstruct
            [Inject] public abstract void PostConstruct();
            #endregion
    
            #region Core
            public abstract void Initialize();
            public abstract void Dispose();
            #endregion
        }
    }
    ```
  * Command: If the Command class has a signal structure attached to it and this signal is fired, it runs the Execute method. Thanks to the Command class, MVC architectural pattern and signal-oriented development are applied together.
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.MVC.Controller
    {
        public abstract class Command<TSignal>
        where TSignal : struct
        {
            #region Core
            public abstract void Execute(TSignal signal);
            #endregion
        }
    }
    ```
  * ViewMediatorInstaller: It is a Zenject Installer specially developed to ensure that the one-to-one relationship between the Mediator class and the View component can be implemented in cases where there is more than one View component in the game scene.
    ```csharp
    using System;
    using System.Collections.Generic;
    using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.MVC.Installers
    {
        public class ViewMediatorInstaller<TView, TMediator> : Installer
            where TView : View.View
            where TMediator : Mediator<TView>
        {
            #region ReadonlyFields
            private readonly List<TView> _views = new();
            #endregion
    
            #region Bindings
            public override void InstallBindings()
            {
                var views = Object.FindObjectsByType<TView>(FindObjectsSortMode.InstanceID);
                
                foreach (var view in views)
                {
                    _views.Add(view);
                    Container.Bind<TView>().FromInstance(view).AsTransient();
                }
    
                Container.BindInterfacesAndSelfTo<ViewMediatorInitializer<TView, TMediator>>().AsSingle().WithArguments(_views);
            }
            #endregion
        }
    
        public sealed class ViewMediatorInitializer<TView, TMediator> : IInitializable, IDisposable
            where TView : View.View
            where TMediator : Mediator<TView>
        {
            #region ReadonlyFields
            private readonly DiContainer _container;
            private readonly List<TView> _views;
            private readonly List<TMediator> _mediators = new();
            #endregion
    
            #region Constructor
            public ViewMediatorInitializer(DiContainer container, List<TView> views)
            {
                _container = container;
                _views = views;
            }
            #endregion
    
            #region Core
            public void Initialize() => _views.ForEach(Bindings);
            public void Dispose() => _mediators.ForEach(x => x.Dispose());
            #endregion
    
            #region Executes
            private void Bindings(TView view)
            {
                DiContainer subContainer = _container.CreateSubContainer();
                    
                subContainer.Bind<TView>().FromInstance(view).AsSingle();
    
                TMediator mediator = subContainer.Instantiate<TMediator>();
                _mediators.Add(mediator);
                    
                subContainer.Bind<TMediator>().FromInstance(mediator).AsSingle();
                subContainer.QueueForInject(mediator);
                
                mediator.Initialize();
            }
            #endregion
        }
    }
    ```
* ProDebug: Contains classes that provides writing log messages in color and different formats.
  * Colorize: This class is a utility for coloring text in Unity's Debug.Log messages and other text outputs, supporting both RGB and HEX color formats.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.ProDebug.Colorize
    {
        public sealed class Colorize
        {
            #region Colors
            public static Colorize Red = new(Color.red);
            public static Colorize Yellow = new(Color.yellow);
            public static Colorize Green = new(Color.green);
            public static Colorize Blue = new(Color.blue);
            public static Colorize Cyan = new(Color.cyan);
            public static Colorize Magenta = new(Color.magenta);
            #endregion
    
            #region HexColors
            public static Colorize Orange = new("#FFA500");
            public static Colorize Olive = new("#808000");
            public static Colorize Purple = new("#800080");
            public static Colorize DarkRed = new("#8B0000");
            public static Colorize DarkGreen = new("#006400");
            public static Colorize DarkOrange = new("#FF8C00");
            public static Colorize Gold = new("#FFD700");
            #endregion
    
            #region Constants
            private const string Suffix = "</color>";
            #endregion
            
            #region Fields
            private readonly string _prefix;
            #endregion
    
            #region Constructors
            private Colorize(Color color) => _prefix = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
            private Colorize(string hexColor) => _prefix = $"<color={hexColor}>";
            #endregion
    
            #region Operators
            public static string operator %(string text, Colorize color) => color._prefix + text + Suffix;
            #endregion
        }
    }
    ```
  * TextFormat: This class is a utility for formatting text in Unity's Debug.Log messages and other text outputs, allowing text to be bold or italic.
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.ProDebug.TextFormat
    {
        public sealed class TextFormat
        {
            #region Fields
            public static TextFormat Bold = new("b");
            public static TextFormat Italic = new("i");
            private readonly string _prefix;
            private readonly string _suffix;
            #endregion
    
            #region Constructor
            private TextFormat(string format)
            {
                _prefix = $"<{format}>";
                _suffix = $"</{format}>";
            }
            #endregion
    
            #region Operators
            public static string operator %(string text, TextFormat textFormat) => textFormat._prefix + text + textFormat._suffix;
            #endregion
        }
    }
    ```
* Utilities: Contains utility classes that are useful and frequently needed in game development processes.
  * Extensions: This class adds extension methods to the Transform component in Unity, enabling axis-based position setting, looking at a target along a specific axis, gradual look direction adjustment, hierarchical object searching, and isometric transformation.
    ```csharp
    using UnityEngine;

    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.Extensions
    {
        public static class Extensions
        {
            public static Transform SetAxes(this Transform transform, float? x = null, float? y = null, float? z = null, bool local = false)
            {
                Vector3 pos = local ? transform.localPosition : transform.position;
    
                if (x != null)
                    pos.x = x.Value;
                if (y != null)
                    pos.y = y.Value;
                if (z != null)
                    pos.z = z.Value;
    
                if (local)
                    transform.localPosition = pos;
                else
                    transform.position = pos;
    
                return transform;
            }
            public static void LookAtWithAxis(this Transform transform, Transform target, Vector3 axis, float angleOffset = 0f)
            {
                transform.LookAt(target);
                transform.Rotate(axis, angleOffset, Space.Self);
            }
            public static void LookAtGradually(this Transform transform, Transform target, Vector3 axis, float maxRadiansDelta, bool stableUpVector = false)
            {
                Vector3 dir = target.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, maxRadiansDelta, 0.0f);
    
                transform.rotation = Quaternion.LookRotation(newDir);
                if (stableUpVector)
                    transform.rotation = Quaternion.Euler(axis.normalized * transform.rotation.eulerAngles.magnitude);
            }
            public static Transform FindRecursive(this Transform transform, string name, bool includeInactive = false)
            {
                foreach (Transform child in transform.GetComponentsInChildren<Transform>(includeInactive))
                {
                    if (child.name.Equals(name))
                        return child;
                }
                return null;
            }
            public static Vector3 InputToIso(this Vector3 input, Matrix4x4 matrix4X4) => matrix4X4.MultiplyPoint3x4(input); 
        }
    }
    ```
  * FPSDisplay: This component is a Unity component that displays the FPS (frames per second) value on the screen and updates it at set intervals for performance monitoring.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.FPSDisplay
    {
        public sealed class FPSDisplay: MonoBehaviour
        {
            #region Fields
            [Header("FPS Display Settings")]
            [SerializeField] private bool show = true;
            [SerializeField] private Rect rect = new(960, 540, 128, 64);
            [Range(0f, 1f)][SerializeField] private float updateInterval = .5f;
            private float _accum;
            private int _frames;
            private float _timeLeft;
            private float _fps;
            private readonly GUIStyle _textStyle = new();
            #endregion
    
            #region Core
            private void Initialize()
            {
                _timeLeft = updateInterval;
    
                _textStyle.fontStyle = FontStyle.Bold;
                _textStyle.normal.textColor = Color.white;
            }
            private void Start() => Initialize();
            #endregion
    
            #region Calculate
            private void CalculateFPS()
            {
                _timeLeft -= Time.deltaTime;
                _accum += Time.timeScale / Time.deltaTime;
                _frames++;
    
                if (_timeLeft <= 0)
                {
                    _fps = _accum / _frames;
                    _timeLeft = updateInterval;
                    _accum = 0f;
                    _frames = 0;
                }
            }
            #endregion
    
            #region Updates
            private void Update() => CalculateFPS();
            #endregion
    
            #region OnGUI
            private void OnGUI()
            {
                if (!show)
                    return;
    
                GUI.Label(rect, _fps.ToString("F2") + "FPS", _textStyle);
            }
            #endregion
        }
    }
    ```
  * TextFormatter: This class is a utility component that manages number and time formatting, providing methods to convert numbers into readable formats and support various time display formats.
    ```csharp
    using System;
    using CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.TextFormatter
    {
        public static class TextFormatter
        {
            #region Constants
            private const string Days = "{0:00}:";
            private const string Hours = "{1:00}:";
            private const string Minutes = "{2:00}:";
            private const string Seconds = "{3:00}:";
            private const string Milliseconds = "{4:00}";
            #endregion
    
            #region Formats
            public static string FormatNumber(int num)
            {
                if (num >= 100000000)
                {
                    return (num / 1000000D).ToString("0.#M");
                }
                if (num >= 1000000)
                {
                    return (num / 1000000D).ToString("0.##M");
                }
                if (num >= 100000)
                {
                    return (num / 1000D).ToString("0.#k");
                }
                if (num >= 10000)
                {
                    return (num / 1000D).ToString("0.##k");
                }
    
                return num.ToString("#,0");
            }
            public static string FormatTime(double totalSecond, TimeFormattingTypes timeFormattingType = TimeFormattingTypes.DaysHoursMinutesSeconds, bool withMilliSeconds = true)
            {
                TimeSpan time = TimeSpan.FromSeconds(totalSecond);
                int days = time.Days;
                int hours = time.Hours;
                int minutes = time.Minutes;
                int seconds = time.Seconds;
                int milliSeconds = (int)(totalSecond * 100);
                milliSeconds %= 100;
    
                bool withDays = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds;
                bool withHours = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.HoursMinutesSeconds;
                bool withMinutes = timeFormattingType == TimeFormattingTypes.DaysHoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.HoursMinutesSeconds || timeFormattingType == TimeFormattingTypes.MinutesSeconds;
    
                string d = withDays ? Days : null;
                string h = withHours ? Hours : null;
                string m = withMinutes ? Minutes : null;
                string s = Seconds;
                string ms = withMilliSeconds ? Milliseconds : null;
    
                string result = string.Format(d + h + m + s + ms, days, hours, minutes, seconds, milliSeconds);
                result = result.TrimEnd(':');
                return result;
            }
            #endregion
        }
    }
    ```
  * TimeCalculator: This class provides various time calculation and formatting utility methods to manage local and server-based date/time operations, including computing time differences, adding time, and tracking elapsed time.
    ```csharp
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums;
    using UnityEngine;
    using UnityEngine.Networking;
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.TimeCalculator
    {
        public static class TimeCalculator
        {
            #region Constants
            private const int DayAsSecond = 86400;
            private const int HourAsSecond = 3600;
            private const int HourAsDay = 24;
            private const int SecondAsDay = 60;
            private const string TimeApiUrl = "https://www.google.com";
            #endregion
    
            #region Fields
            private static float _time;
            private static DateTime _cachedDateTime;
            #endregion
    
            #region Core
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
            private static void Initialize()
            {
                _cachedDateTime = default;
                // await RefreshDateTimeAsync();
            }
            #endregion
    
            #region Local
            public static double SubtractCurrent(string time, DateTypes dateType = DateTypes.Now)
            {
                DateTime localDate = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
    
                DateTime.TryParse(time, out var timeDate);
    
                double timeValue = TimeSpan.FromTicks(timeDate.Ticks).TotalSeconds;
                double currentVal = TimeSpan.FromTicks(localDate.Ticks).TotalSeconds;
    
                return (currentVal - timeValue);
            }
            public static DateTime ConvertToDateTime(string timeStr)
            {
                DateTime.TryParse(timeStr, out var newDate);
                return newDate;
            }
            public static double Subtract(string oldTime, string newTime)
            {
                DateTime.TryParse(newTime, out var newDate);
                DateTime.TryParse(oldTime, out var oldDate);
    
                double old = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;
                double now = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;
    
                return (now - old);
            }
            public static double Addition(string time, string addedTime)
            {
                DateTime.TryParse(time, out var newDate);
                DateTime.TryParse(addedTime, out var oldDate);
    
                double click = TimeSpan.FromTicks(newDate.Ticks).TotalSeconds;
                double server = TimeSpan.FromTicks(oldDate.Ticks).TotalSeconds;
    
                return (server + click);
            }
            public static string AddTime(string clickTime, int timeStepAsSecond)
            {
                DateTime.TryParse(clickTime, out var clickDate);
    
                clickDate = clickDate.AddSeconds(timeStepAsSecond);
                var calculatedDate = clickDate.ToString("yyyy/MM/dd HH:mm:ss");
    
                return calculatedDate;
            }
            public static string AddTime(string localTime, object notificationTimeFreq)
            {
                throw new NotImplementedException();
            }
            public static string GetLocalTimeAsString(DateTypes dateType = DateTypes.Now)
            {
                DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
                string result = now.ToString("yyyy/MM/dd HH:mm:ss");
    
                return result;
            }
            public static DateTime GetLocalTime(DateTypes dateType = DateTypes.Now)
            {
                DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
                return now;
            }
            public static int GetLocalTimeHour(DateTypes dateType = DateTypes.Now)
            {
                DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
                return now.Hour;
            }
            public static string GetHourAsString(DateTypes dateType = DateTypes.Now)
            {
                DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
                int h = now.Hour;
                string hourStr;
    
                if (h < 10)
                {
                    hourStr = "0" + h;
                }
                else
                {
                    hourStr = h.ToString();
                }
    
                return hourStr;
            }
            public static string GetHourSpaceAsString(DateTypes dateType = DateTypes.Now)
            {
                DateTime now = dateType == DateTypes.Now ? DateTime.Now : DateTime.UtcNow;
                int h1 = now.Hour;
                int h2 = h1 + 1;
    
                string hstr1;
                string hstr2;
    
                if (h1 < 10)
                    hstr1 = "0" + h1;
                else
                    hstr1 = h1.ToString();
    
    
                if (h2 < 10)
                    hstr2 = "0" + h2;
                else
                    hstr2 = h2.ToString();
    
    
                return hstr1 + "_" + hstr2;
            }
            #endregion
    
            #region Timer
            public static void StartTimer()
            {
                _time = Time.realtimeSinceStartup;
            }
            public static float StopTimer(string title = "")
            {
                float diff = Time.realtimeSinceStartup - _time;
                diff = diff < 0 ? 0 : diff;
                Debug.Log(title + "TIME::" + diff);
                return diff;
            }
            #endregion
    
            #region Fetch
            public static async Task RefreshDateTimeAsync()
            {
                var localDate = await FetchCurrentLocalDate();
                Debug.Log($"Current local date: {localDate}");
                _cachedDateTime = localDate;
                await Task.Yield();
            }
            private static async Task<DateTime> FetchCurrentLocalDate()
            {
                using UnityWebRequest request = new UnityWebRequest(TimeApiUrl, UnityWebRequest.kHttpVerbHEAD);
                request.redirectLimit = 0;
                request.timeout = 10;
    
                UnityWebRequestAsyncOperation asyncOp = request.SendWebRequest();
    
                while (!asyncOp.isDone)
                    await Task.Delay(100);
    
                if (request.result == UnityWebRequest.Result.Success)
                {
                    string dateString = request.GetResponseHeader("Date");
                    DateTime serverUtcDate = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).ToUniversalTime();
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    serverUtcDate.Add(TimeSpan.FromSeconds(-Time.realtimeSinceStartup));
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(serverUtcDate, TimeZoneInfo.Local);
                    return localDate;
                }
                else
                {
                    Debug.LogError($"Error fetching date: {request.error}");
                    return default;
                }
            }
            #endregion
    
            #region Gets
            public static async Task<float> GetStartCountdownTime(int targetHour, DateTime? savedDateTime)
            {
                DateTime localDate = await GetLocalDateFetcher();
    
                // ReSharper disable once PossibleInvalidOperationException
                float startCountdownTime = (float)((targetHour * HourAsSecond) -
                                                   (localDate - savedDateTime)?.TotalSeconds) + 1;
                return startCountdownTime;
            }
            public static async Task<DateTime> GetLocalDateFetcher()
            {
                float currentPlayedTime = Time.realtimeSinceStartup;
                TimeSpan value = TimeSpan.FromSeconds(currentPlayedTime);
                DateTime localDate = await GetCachedDateTime();
                DateTime localDateFetcher = localDate.Add(value);
    
                return localDateFetcher;
            }
            public static async Task<DateTime> GetCachedDateTime()
            {
                if (_cachedDateTime == default)
                    await Task.Run(() => _cachedDateTime != default);
    
                return _cachedDateTime;
            }
            #endregion
        }
    }
    ```
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums
    {
        public enum DateTypes
        {
            Now,
            UtcNow
        }
    }
    ```
    ```csharp
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums
    {
        public enum TimeFormattingTypes
        {
            DaysHoursMinutesSeconds,
            HoursMinutesSeconds,
            MinutesSeconds,
            Seconds
        }
    }
    ```
  * Utilities: This class provides utility functions for Unity, including tag creation, adjusting CanvasGroup alpha, converting world positions to UI screen positions, shuffling lists, bubble sorting, and performing isometric transformations.
    ```csharp
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    #if UNITY_EDITOR
    #endif
    
    namespace CodeCatGames.HiveMind.Core.Runtime.Utilities
    {
        public static class Utilities
        {
    #if UNITY_EDITOR
            public static void CreateTag(string tag)
            {
                var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
                if (asset != null)
                { // sanity checking
                    var so = new SerializedObject(asset);
                    var tags = so.FindProperty("tags");
    
                    var numTags = tags.arraySize;
                    // do not create duplicates
                    for (int i = 0; i < numTags; i++)
                    {
                        var existingTag = tags.GetArrayElementAtIndex(i);
                        if (existingTag.stringValue == tag) return;
                    }
    
                    tags.InsertArrayElementAtIndex(numTags);
                    tags.GetArrayElementAtIndex(numTags).stringValue = tag;
                    so.ApplyModifiedProperties();
                    so.Update();
                }
            }
    #endif
            public static IEnumerator SetCanvasGroupAlpha(CanvasGroup canvasGroup, float targetValue, float duration = 1f)
            {
                float t = 0f;
                float startValue = canvasGroup.alpha;
                while (t < 1f)
                {
                    t += Time.deltaTime / duration;
                    canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);
                    yield return null;
                }
            }
            public static Vector3 WorldToScreenPointForUICamera(Vector3 worldPos, Camera gameCamera, Canvas screenCanvas)
            {
                Vector3 canvasPos;
                Vector3 screenPos = gameCamera.WorldToScreenPoint(worldPos);
    
                if (screenCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    canvasPos = screenPos;
                else
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    screenCanvas.transform as RectTransform, screenPos, screenCanvas.worldCamera, out var posRect2D);
                    canvasPos = screenCanvas.transform.TransformPoint(posRect2D);
                }
    
                return canvasPos;
            }
            public static List<T> Shuffle<T>(List<T> ts)
            {
                var newList = ts;
                var count = newList.Count;
                var last = count - 1;
                for (int i = 0; i < last; i++)
                {
                    var r = Random.Range(i, count);
                    (newList[r], newList[i]) = (newList[i], newList[r]);
                }
                return newList;
            }
            public static IList<int> BubbleSort(IList<int> ts)
            {
                var newList = ts;
                int count = newList.Count;
                for (int i = 0; i < count - 1; i++)
                    for (int j = 0; j < count - 1; j++)
                        if (newList[j] > newList[j + 1])
                            (newList[j], newList[j + 1]) = (newList[j + 1], newList[j]);
                
                return newList;
            }
            public static Matrix4x4 IsoMatrix(Quaternion rotate) => Matrix4x4.Rotate(rotate);
        }
    }
    ```

## Samples
* Helpers: Contains components that demonstrating how to use Helpers components and classes.
  * CountdownSample: This component demonstrating how to use the Countdown class.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.Utilities.Enums;
    using TMPro;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Countdown
    {
        public sealed class CountdownSample : MonoBehaviour
        {
            #region Fields
            [Header("Countdown Sample Fields")]
            [SerializeField] private TextMeshProUGUI countdownSampleText;
            [SerializeField] private TMP_InputField addSecondsField;
            private Core.Runtime.Helpers.Countdown.Countdown _countdown;
            #endregion
    
            #region Core
            private void Awake()
            {
                _countdown = new Core.Runtime.Helpers.Countdown.Countdown();
                
                _countdown.Setup(TimeFormattingTypes.DaysHoursMinutesSeconds, true, 3600);
            }
            #endregion
    
            #region Executes
            public void SetPause(bool isPause) => _countdown?.SetPause(isPause);
            public void AddSeconds() => _countdown?.AddSeconds(int.Parse(addSecondsField.text));
            #endregion
    
            #region Receivers
            private void OnCountdownEnded() => Debug.Log("Countdown Ended!");
            #endregion
    
            #region Update
            private void Update()
            {
                _countdown?.ExternalUpdate(OnCountdownEnded);
    
                countdownSampleText.SetText($"{_countdown?.GetFormattedTime()}");
            }
            #endregion
        }
    }
    ```
  * ExploderSample: This component demonstrating how to use the Exploder component.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Exploder
    {
        public sealed class ExploderSample : MonoBehaviour
        {
            #region Fields
            [Header("Exploder Sample Fields")]
            [SerializeField] private Core.Runtime.Helpers.Exploder.Exploder exploder;
            #endregion
    
            #region Executes
            [ContextMenu("Explode")]
            public void Explode() => exploder?.Explode();
            [ContextMenu("Refresh")]
            public void Refresh() => exploder?.Refresh();
            #endregion
        }
    }
    ```
  * FollowerSample: This component demonstrating how to use the Follower component.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Follower
    {
        public sealed class FollowerSample : MonoBehaviour
        {
            #region Fields
            [Header("Follower Sample Fields")]
            [SerializeField] private Core.Runtime.Helpers.Follower.Follower follower;
            [SerializeField] private Transform followerObject;
            [SerializeField] private Transform targetObject;
            [SerializeField] private bool withSnap;
            #endregion
    
            #region Core
            private void Awake() => follower?.Initialize(followerObject, targetObject, withSnap);
            #endregion
    
            #region Executes
            [ContextMenu("Can Follow")]
            public void CanFollow() => follower?.SetCanFollow(true);
            [ContextMenu("Cant Follow")]
            public void CantFollow() => follower?.SetCanFollow(false);
            #endregion
    
            #region Update
            private void Update() => follower?.ExternalUpdate();
            #endregion
        }
    }
    ```
  * PhysicSample: This component demonstrating how to use the Physics components.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.Helpers.Physics;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Physics
    {
        public sealed class PhysicSample : MonoBehaviour
        {
            #region Fields
            [Header("Physic Sample Fields")]
            [SerializeField] private ContactListener3D contactListener;
            #endregion
    
            #region Core
            private void Awake()
            {
                contactListener.EnterCallBack += OnEnterCallBack;
                contactListener.StayCallBack += OnStayCallBack;
                contactListener.ExitCallBack += OnExitCallBack;
            }
            private void OnDestroy()
            {
                contactListener.EnterCallBack -= OnEnterCallBack;
                contactListener.StayCallBack -= OnStayCallBack;
                contactListener.ExitCallBack -= OnExitCallBack;
            }
            #endregion
    
            #region Receivers
            private void OnEnterCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Enter call callback");
            private void OnStayCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Stay call callback");
            private void OnExitCallBack(Collision arg1, Collision2D arg2, Collider arg3, Collider2D arg4) => Debug.Log("Exit call callback");
            #endregion
        }
    }
    ```
  * RotatorSample: This component demonstrating how to use the Rotator component.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.Rotator
    {
        public sealed class RotatorSample : MonoBehaviour
        {
            #region Fields
            [Header("Rotator Sample Fields")]
            [SerializeField] private Core.Runtime.Helpers.Rotator.Rotator rotator;
            #endregion
            
            #region Executes
            [ContextMenu("Can Rotate")]
            public void Explode() => rotator?.SetCanRotate(true);
            [ContextMenu("Cant Rotate")]
            public void Refresh() => rotator?.SetCanRotate(false);
            #endregion
    
            #region Update
            private void Update() => rotator?.ExternalUpdate();
            #endregion
        }
    }
    ```
  * SlowMotionSample: This component demonstrating how to use the SlowMotion class.
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.Helpers.SlowMotion
    {
        public sealed class SlowMotionSample : MonoBehaviour
        {
            #region Fields
            [Header("Slow Motion Sample Fields")]
            [SerializeField] private float slowMotionFactor;
            private Core.Runtime.Helpers.SlowMotion.SlowMotion _slowMotion;
            #endregion
    
            #region Core
            private void Awake()
            {
                _slowMotion = new Core.Runtime.Helpers.SlowMotion.SlowMotion();
                
                _slowMotion?.Setup(slowMotionFactor);
            }
            #endregion
    
            #region Executes
            public void Activate() => _slowMotion?.Activate();
            public void DeActivate() => _slowMotion?.DeActivate();
            #endregion
        }
    }
    ```
* MVC: Contains components and classes that demonstrating how to use MVC components and classes.
  * Models & Data: This component and class demonstrating how to use the Model class.
    ```csharp
    using UnityEngine;

    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Data.ScriptableObjects
    {
        [CreateAssetMenu(fileName = "MvcSampleSettings", menuName = "CodeCatGames/HiveMind/Samples/MVC/MvcSampleSettings", order = 0)]
        public sealed class MvcSampleSettings : ScriptableObject
        {
            #region Fields
            [Header("Mvc Sample Settings Fields")]
            [SerializeField] private Color[] colors;
            [SerializeField] private string[] colorNames;
            #endregion
    
            #region Getters
            public Color[] Colors => colors;
            public string[] ColorNames => colorNames;
            #endregion
        }
    }
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Data.ScriptableObjects;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Model
    {
        public sealed class MvcSampleModel : Model<MvcSampleSettings>
        {
            #region Constants
            private const string ResourcePath = "Samples/MVC/MvcSampleSettings";
            #endregion
            
            #region Constructor
            public MvcSampleModel() : base(ResourcePath) { }
            #endregion
            
            #region PostConstruct
            public override void PostConstruct() { }
            #endregion
        }
    }
    ```
  * Views & Mediators: These components and classes demonstrating how to use the View component and Mediator class.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
    {
        public sealed class ChangeColorObjectView : View
        {
            #region Fields
            [Header("Change Color Object View Fields")]
            [SerializeField] private MeshRenderer meshRenderer;
            #endregion
    
            #region Getters
            public MeshRenderer MeshRenderer => meshRenderer;
            #endregion
        }
    }
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
    using Zenject;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
    {
        public sealed class ChangeColorObjectMediator : Mediator<ChangeColorObjectView>
        {
            #region ReadonlyFields
            private readonly SignalBus _signalBus;
            #endregion
            
            #region Constructor
            public ChangeColorObjectMediator(ChangeColorObjectView view, SignalBus signalBus) : base(view) => _signalBus = signalBus;
            #endregion
    
            #region PostConstruct
            public override void PostConstruct() { }
            #endregion
            
            #region Core
            public override void Initialize() => _signalBus.Subscribe<ChangeColorSignal>(OnChangeColorSignal);
            public override void Dispose() => _signalBus.Unsubscribe<ChangeColorSignal>(OnChangeColorSignal);
            #endregion
    
            #region SignalReceivers
            private void OnChangeColorSignal(ChangeColorSignal signal) => GetView.MeshRenderer.material.color = signal.Color;
            #endregion
        }
    }
    ```
    ```csharp
    using System;
    using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
    {
        public sealed class ColorChangerView : View
        {
            #region Actions
            public event Action<int> ClickChangeColorButton; 
            #endregion
            
            #region Fields
            [Header("Color Changer View Fields")]
            [SerializeField] private Button[] changeColorButtons;
            [SerializeField] private TextMeshProUGUI[] changeColorButtonTexts;
            #endregion
    
            #region Getters
            public Button[] ChangeColorButtons => changeColorButtons;
            public TextMeshProUGUI[] ChangeColorButtonTexts => changeColorButtonTexts;
            #endregion
    
            #region ButtonReceivers
            public void OnChangeColorButtonClicked(int index) => ClickChangeColorButton?.Invoke(index);
            #endregion
        }
    }
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Model;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Views
    {
        public sealed class ColorChangerMediator : Mediator<ColorChangerView>
        {
            #region ReadonlyFields
            private readonly SignalBus _signalBus;
            private readonly MvcSampleModel _mvcSampleModel;
            #endregion
            
            #region Constructor
            public ColorChangerMediator(ColorChangerView view, SignalBus signalBus, MvcSampleModel mvcSampleModel) : base(view)
            {
                _signalBus = signalBus;
                _mvcSampleModel = mvcSampleModel;
            }
            #endregion
            
            #region PostConstruct
            public override void PostConstruct() { }
            #endregion
            
            #region Core
            public override void Initialize()
            {
                GetView.ClickChangeColorButton += OnChangeColorButtonClicked;
                
                SetVisual();
            }
            public override void Dispose() => GetView.ClickChangeColorButton -= OnChangeColorButtonClicked;
            #endregion
    
            #region ButtonReceivers
            private void OnChangeColorButtonClicked(int index) =>
                _signalBus.Fire(new ChangeColorSignal(_mvcSampleModel.GetSettings.Colors[index]));
            #endregion
    
            #region Executes
            private void SetVisual()
            {
                Button[] buttons = GetView.ChangeColorButtons;
                TextMeshProUGUI[] texts = GetView.ChangeColorButtonTexts;
                
                for (int i = 0; i < buttons.Length; i++)
                    buttons[i].targetGraphic.color = _mvcSampleModel.GetSettings.Colors[i];
                
                for (int i = 0; i < texts.Length; i++)
                    texts[i].SetText($"{_mvcSampleModel.GetSettings.ColorNames[i]}");
            }
            #endregion
        }
    }
    ```
  * Commands & Signals: These classes and structs demonstrating how to use the Command class and performs the signal-oriented development.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers
    {
        public sealed class InitializeCommand : Command<InitializeSignal>
        {
            #region Executes
            public override void Execute(InitializeSignal signal) => Debug.Log("MVC Sample Initialize Command Executed!");
            #endregion
        }
    }
    ```
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers
    {
        public sealed class ChangeColorCommand : Command<ChangeColorSignal>
        {
            #region Executes
            public override void Execute(ChangeColorSignal signal) => Debug.Log("Change Color Command Executed!");
            #endregion
        }
    }
    ```
    ```csharp
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals
    {
        public readonly struct InitializeSignal { } //Has Command
        public readonly struct ChangeColorSignal
        {
            #region Fields
            private readonly Color _color;
            #endregion
    
            #region Getters
            public Color Color => _color;
            #endregion
    
            #region Constructor
            public ChangeColorSignal(Color color) => _color = color;
            #endregion
        }
    }
    ```
  * Installers: This component demonstrating how to use the ViewMediatorInstaller class and performs the binding operations for Zenject.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Controllers;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Model;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Signals;
    using CodeCatGames.HiveMind.Samples.Runtime.MVC.Views;
    using Zenject;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.MVC.Installers
    {
        public sealed class MvcSampleMonoInstaller : MonoInstaller
        {
            #region Bindings
            public override void InstallBindings()
            {
                Container.BindInterfacesAndSelfTo<MvcSampleModel>().AsSingle().NonLazy();
                
                Container.Bind<ColorChangerView>().FromComponentsInHierarchy().AsSingle().NonLazy();
                Container.BindInterfacesAndSelfTo<ColorChangerMediator>().AsSingle().NonLazy();
    
                Container.Install<ViewMediatorInstaller<ChangeColorObjectView, ChangeColorObjectMediator>>();
    
                Container.DeclareSignal<InitializeSignal>();
                Container.DeclareSignal<ChangeColorSignal>();
    
                Container.BindInterfacesAndSelfTo<InitializeCommand>().AsSingle().NonLazy();
                Container.BindInterfacesAndSelfTo<ChangeColorCommand>().AsSingle().NonLazy();
    
                Container.BindSignal<InitializeSignal>().ToMethod<InitializeCommand>((x, s) => x.Execute(s)).FromResolve();
                Container.BindSignal<ChangeColorSignal>().ToMethod<ChangeColorCommand>((x, s) => x.Execute(s))
                    .FromResolve();
            }
            #endregion
    
            #region Cycle
            public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeSignal());
            #endregion
        }
    }
    ```
* ProDebug: Contains component that demonstrating how to use ProDebug classes.
  * ProDebugSample: This component demonstrating how to use the Colorize and TextFormat classes.
    ```csharp
    using CodeCatGames.HiveMind.Core.Runtime.ProDebug.Colorize;
    using CodeCatGames.HiveMind.Core.Runtime.ProDebug.TextFormat;
    using TMPro;
    using UnityEngine;
    
    namespace CodeCatGames.HiveMind.Samples.Runtime.ProDebug
    {
        public sealed class ProDebugSample : MonoBehaviour
        {
            #region Fields
            [Header("Pro Debug Sample Fields")]
            [SerializeField] private TMP_Dropdown colorizeDropdown;
            [SerializeField] private TMP_Dropdown textFormatDropdown;
            private int _colorizeDropdownValue;
            private int _textFormatDropdownValue;
            #endregion
            
            #region ButtonReceivers
            public void OnColorizeDropdownValueChanged() => _colorizeDropdownValue = colorizeDropdown.value;
            public void OnTextFormatDropdownValueChanged() => _textFormatDropdownValue = textFormatDropdown.value;
            public void OnShowDebugButtonClicked()
            {
                Colorize colorize = _colorizeDropdownValue switch
                {
                    0 => Colorize.Red,
                    1 => Colorize.Yellow,
                    2 => Colorize.Green,
                    3 => Colorize.Blue,
                    4 => Colorize.Cyan,
                    5 => Colorize.Magenta,
                    6 => Colorize.Orange,
                    7 => Colorize.Olive,
                    8 => Colorize.Purple,
                    9 => Colorize.DarkRed,
                    10 => Colorize.DarkGreen,
                    11 => Colorize.DarkOrange,
                    12 => Colorize.Gold,
                    _ => null
                };
    
                TextFormat textFormat = _textFormatDropdownValue switch
                {
                    0 => TextFormat.Bold,
                    1 => TextFormat.Italic,
                    _ => null
                };
                
                string logMessage = "Log Message" % colorize % textFormat;
                
                Debug.Log(logMessage);
            }
            #endregion
        }
    }
    ```
* SampleGame: It uses HiveMind's components and classes, as well as its own components and classes. It is a template that shows how to develop a game with HiveMind.
  * StartupSceneLoader: This class automatically loads the startup scene when playing the game in the Unity editor and returns to the previous scene after exiting play mode.
    ```csharp
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine.SceneManagement;
    
    namespace CodeCatGames.HiveMind.Samples.Editor.SampleGame
    {
        [InitializeOnLoad]
        public class StartupSceneLoader
        {
            #region Constants
            private const string PreviousSceneKey = "PreviousScene";
            private const string ShouldLoadStartupSceneKey = "LoadStartupScene";
            private const string LoadStartupSceneOnPlay = "HiveMind/Samples/SampleGame/Load Startup Scene On Play";
            private const string DontLoadStartupSceneOnPlay = "HiveMind/Samples/SampleGame/Don't Load Startup Scene On Play";
            #endregion
    
            #region Fields
            private static bool _restartingToSwitchedScene;
            #endregion
    
            #region Getters
            private static string StartupScene => EditorBuildSettings.scenes[0].path;
            #endregion
    
            #region Props
            private static string PreviousScene
            {
                get => EditorPrefs.GetString(PreviousSceneKey);
                set => EditorPrefs.SetString(PreviousSceneKey, value);
            }
            private static bool ShouldLoadStartupScene
            {
                get
                {
                    if (!EditorPrefs.HasKey(ShouldLoadStartupSceneKey))
                        EditorPrefs.SetBool(ShouldLoadStartupSceneKey, true);
    
                    return EditorPrefs.GetBool(ShouldLoadStartupSceneKey);
                }
    
                set => EditorPrefs.SetBool(ShouldLoadStartupSceneKey, value);
            }
            #endregion
    
            #region Constructor
            static StartupSceneLoader() =>
                EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
            #endregion
    
            #region MenuItems
            [MenuItem(LoadStartupSceneOnPlay, true)]
            private static bool ShowLoadStartupSceneOnPlay() => !ShouldLoadStartupScene;
            [MenuItem(LoadStartupSceneOnPlay)]
            private static void EnableLoadStartupSceneOnPlay() => ShouldLoadStartupScene = true;
            [MenuItem(DontLoadStartupSceneOnPlay, true)]
            private static bool ShowDoNotLoadStartupSceneOnPlay() => ShouldLoadStartupScene;
            [MenuItem(DontLoadStartupSceneOnPlay)]
            private static void DisableDoNotLoadBootstrapSceneOnPlay() => ShouldLoadStartupScene = false;
            #endregion
    
            #region Executes
            private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange playModeStateChange)
            {
                if (!ShouldLoadStartupScene)
                    return;
    
                if (_restartingToSwitchedScene) //error check as multiple starts and stops happening
                {
                    if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
                        _restartingToSwitchedScene = false;
                    
                    return;
                }
    
                if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
                {
                    // cache previous scene to return to it after play session ends
                    PreviousScene = SceneManager.GetActiveScene().path;
    
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        // user either hit "Save" or "Don't Save"; open bootstrap scene
    
                        if (!string.IsNullOrEmpty(StartupScene) && System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == StartupScene))
                        {
                            Scene activeScene = SceneManager.GetActiveScene();
    
                            _restartingToSwitchedScene = activeScene.path == string.Empty || !StartupScene.Contains(activeScene.path);
    
                            // only switch if editor is in an empty scene or active scene is not startup scene
                            if (_restartingToSwitchedScene)
                            {
                                EditorApplication.isPlaying = false;
    
                                // scene is included in build settings; open it
                                EditorSceneManager.OpenScene(StartupScene);
    
                                EditorApplication.isPlaying = true;
                            }
                        }
                    }
                    else
                    {
                        // user either hit "Cancel" or exited window; don't open startup scene & return to editor
                        EditorApplication.isPlaying = false;
                    }
                }
                //return to last open scene
                else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
                {
                    if (!string.IsNullOrEmpty(PreviousScene))
                        EditorSceneManager.OpenScene(PreviousScene);
                }
            }
            #endregion
        }
    }
    ```
  * Application: Contains components and classes of application management. Located in Zenject's ProjectContext.
    * Installers: These components and classes perform the Zenject bindings required for application management.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
      {
          public sealed class ApplicationMonoInstaller : MonoInstaller
          {
              #region Bindings
              public override void InstallBindings()
              {
                  SignalBusInstaller.Install(Container);
      
                  Container.Install<ApplicationModelInstaller>();
                  Container.Install<ApplicationSignalInstaller>();
              }
              #endregion
      
              #region Cycle
              public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeApplicationSignal());
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
      {
          public sealed class ApplicationModelInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings() =>
                  Container.BindInterfacesAndSelfTo<ApplicationModel>().AsSingle().NonLazy();
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Application
      {
          public sealed class ApplicationSignalInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.DeclareSignal<InitializeApplicationSignal>();
                  Container.DeclareSignal<AppQuitSignal>();
      
                  Container.BindInterfacesAndSelfTo<InitializeApplicationCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<AppQuitCommand>().AsSingle().NonLazy();
      
                  Container.BindSignal<InitializeApplicationSignal>()
                      .ToMethod<InitializeApplicationCommand>((x, s) => x.Execute(s)).FromResolve();
                  Container.BindSignal<AppQuitSignal>().ToMethod<AppQuitCommand>((x, s) => x.Execute(s)).FromResolve();
              }
              #endregion
          }
      }
      ```
    * Models & Data: This component and class contains the data needed in application management.
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Application
      {
          [CreateAssetMenu(fileName = "ApplicationSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/Application/ApplicationSettings")]
          public sealed class ApplicationSettings : ScriptableObject
          {
              #region Fields
              [Header("Application Settings Fields")]
              [Range(30, 240)][SerializeField] private int targetFrameRate;
              [SerializeField] private bool runInBackground;
              #endregion
      
              #region Getters
              public int TargetFrameRate => targetFrameRate;
              public bool RunInBackground => runInBackground;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Application;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application
      {
          public sealed class ApplicationModel : Model<ApplicationSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/Application/ApplicationSettings";
              #endregion
      
              #region Constructor
              public ApplicationModel() : base(ResourcePath) { }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
          }
      }
      ```
    * Commands & Signals: These classes and structs are entry and exit points of the application.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Application;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
      using UnityEngine;
      using Screen = UnityEngine.Device.Screen;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application
      {
          public sealed class InitializeApplicationCommand : Command<InitializeApplicationSignal>
          {
              #region ReadonlyFields
              private readonly ApplicationModel _applicationModel;
              #endregion
      
              #region Constructor
              public InitializeApplicationCommand(ApplicationModel applicationModel) => _applicationModel = applicationModel;
              #endregion
      
              #region Executes
              public override void Execute(InitializeApplicationSignal signal)
              {
                  UnityEngine.Application.targetFrameRate = _applicationModel.GetSettings.TargetFrameRate;
                  UnityEngine.Application.runInBackground = _applicationModel.GetSettings.RunInBackground;
      
                  Screen.sleepTimeout = SleepTimeout.NeverSleep;
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
      using UnityEditor;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Application
      {
          public sealed class AppQuitCommand : Command<AppQuitSignal>
          {
              #region ReadonlyFields
              private readonly AudioModel _audioModel;
              private readonly HapticModel _hapticModel;
              private readonly CurrencyModel _currencyModel;
              private readonly LevelModel _levelModel;
              #endregion
      
              #region Constructor
              public AppQuitCommand(AudioModel audioModel, HapticModel hapticModel, CurrencyModel currencyModel, LevelModel levelModel)
              {
                  _audioModel = audioModel;
                  _hapticModel = hapticModel;
                  _currencyModel = currencyModel;
                  _levelModel = levelModel;
              }
              #endregion
      
              #region Executes
              public override void Execute(AppQuitSignal signal)
              {
                  _audioModel.Save();
                  _hapticModel.Save();
                  _currencyModel.Save();
                  _levelModel.Save();
      
      #if UNITY_EDITOR
                  EditorApplication.ExitPlaymode();
      #else
                  UnityEngine.Application.Quit();
      #endif
              }
              #endregion
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application
      {
          public readonly struct InitializeApplicationSignal { } // Has Command
          public readonly struct AppQuitSignal { } // Has Command
      }
      ```
  * CrossScene: Contains components and classes that may need to be used in all scenes of the SampleGame. Located in Zenject's ProjectContext.
    * Installers: These components and classes perform the Zenject binding required for CrossScene's contents.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handlers.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
      using UnityEngine;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
      {
          public sealed class CrossSceneMonoInstaller : MonoInstaller
          {
              #region Fields
              [Header("Factories Fields")]
              [SerializeField] private Transform currencyTrailParent;
              [SerializeField] private GameObject currencyTrailPrefab;
              #endregion
      
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Install<CrossSceneModelInstaller>();
                  Container.Install<CrossSceneMediationInstaller>();
                  Container.Install<CrossSceneSignalInstaller>();
      
                  Container.BindInterfacesAndSelfTo<CurrencyTrailSpawnHandler>().AsSingle().NonLazy();
      
                  Container.BindFactory<CurrencyTrailData, CurrencyTrailMediator, CurrencyTrailFactory>()
                    .FromPoolableMemoryPool<CurrencyTrailData, CurrencyTrailMediator, CurrencyTrailPool>
                    (poolBinder => poolBinder
                        .WithInitialSize(5)
                        .FromSubContainerResolve()
                        .ByNewPrefabInstaller<CurrencyTrailInstaller>(currencyTrailPrefab)
                        .UnderTransform(currencyTrailParent)
                    );
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
      {
          public sealed class CrossSceneModelInstaller: Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.BindInterfacesAndSelfTo<CrossSceneModel>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<AudioModel>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<CurrencyModel>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<HapticModel>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<LevelModel>().AsSingle().NonLazy();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
      {
          public sealed class CrossSceneMediationInstaller: Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Bind<LoadingScreenPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<LoadingScreenPanelMediator>().AsSingle().NonLazy();
      
                  Container.Bind<AudioView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<AudioMediator>().AsSingle().NonLazy();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
      {
          public sealed class CrossSceneSignalInstaller: Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.DeclareSignal<ChangeLoadingScreenActivationSignal>();
                  Container.DeclareSignal<LoadSceneSignal>();
                  Container.DeclareSignal<PlayAudioSignal>();
                  Container.DeclareSignal<PlayHapticSignal>();
                  Container.DeclareSignal<ChangeCurrencySignal>();
                  Container.DeclareSignal<SpawnCurrencyTrailSignal>();
                  Container.DeclareSignal<RefreshCurrencyVisualSignal>();
                  Container.DeclareSignal<SettingsButtonPressedSignal>();
                  Container.DeclareSignal<SettingsButtonRefreshSignal>();
                  Container.DeclareSignal<ChangeUIPanelSignal>();
      
                  Container.BindInterfacesAndSelfTo<ChangeCurrencyCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<LoadSceneCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<PlayHapticCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<SettingsButtonPressedCommand>().AsSingle().NonLazy();
      
                  Container.BindSignal<ChangeCurrencySignal>().ToMethod<ChangeCurrencyCommand>((x, s) => x.Execute(s))
                      .FromResolve();
                  Container.BindSignal<LoadSceneSignal>().ToMethod<LoadSceneCommand>((x, s) => x.Execute(s)).FromResolve();
                  Container.BindSignal<PlayHapticSignal>().ToMethod<PlayHapticCommand>((x, s) => x.Execute(s)).FromResolve();
                  Container.BindSignal<SettingsButtonPressedSignal>()
                      .ToMethod<SettingsButtonPressedCommand>((x, s) => x.Execute(s)).FromResolve();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.CrossScene
      {
          public sealed class CurrencyTrailInstaller : Installer<CurrencyTrailInstaller>
          {
              #region Bindings
              public override void InstallBindings() { }
              #endregion
          }
      }
      ```
    * Factories: These classes are the factory and pool classes required to spawn CurrencyTrail.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories.CrossScene
      {
          public sealed class CurrencyTrailFactory : PlaceholderFactory<CurrencyTrailData, CurrencyTrailMediator> { }
          public sealed class CurrencyTrailPool : MonoPoolableMemoryPool<CurrencyTrailData, IMemoryPool, CurrencyTrailMediator> { }
      }
      ```
    * Handlers: These classes base and specialized handler classes that manage in-game object spawning processes based on signals.
      ```csharp
      using System;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handlers.CrossScene
      {
          public abstract class SpawnHandler<TModel, TFactory> : IDisposable
               where TModel : class
               where TFactory : IPlaceholderFactory
          {
              #region ReadonlyFields
              protected readonly SignalBus SignalBus;
              protected readonly TModel Model;
              protected readonly TFactory Factory;
              #endregion
      
              #region Constructor
              public SpawnHandler(SignalBus signalBus, TModel model, TFactory factory)
              {
                  SignalBus = signalBus;
                  Model = model;
                  Factory = factory;
              }
              #endregion
      
              #region Dispose
              public abstract void Dispose();
              #endregion
      
              #region Subscriptions
              protected abstract void SetSubscriptions(bool isSub);
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Factories.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Handlers.CrossScene
      {
          public sealed class CurrencyTrailSpawnHandler : SpawnHandler<CurrencyModel, CurrencyTrailFactory>
          {
              #region Constructor
              public CurrencyTrailSpawnHandler(SignalBus signalBus, CurrencyModel model, CurrencyTrailFactory factory) :
                  base(signalBus, model, factory) => SetSubscriptions(true);
              #endregion
      
              #region Dispose
              public override void Dispose() => SetSubscriptions(false);
              #endregion
      
              #region Subscriptions
              protected override void SetSubscriptions(bool isSub)
              {
                  if (isSub)
                      SignalBus.Subscribe<SpawnCurrencyTrailSignal>(OnSpawnCurrencyTrailSignal);
                  else
                      SignalBus.Unsubscribe<SpawnCurrencyTrailSignal>(OnSpawnCurrencyTrailSignal);
              }
              #endregion
      
              #region SignalReceivers
              private void OnSpawnCurrencyTrailSignal(SpawnCurrencyTrailSignal signal) => SpawnCurrencyTrailProcess(signal);
              #endregion
      
              #region Executes
              private void SpawnCurrencyTrailProcess(SpawnCurrencyTrailSignal signal) =>
                  Factory.Create(signal.CurrencyTrailData);
              #endregion
          }
      }
      ```
    * Enums: These enums are used in CrossScene's components and classes.
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum AudioTypes : int
          {
              Music = 0,
              Sound = 1,
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum CurrencyTypes : int
          {
              Coin = 0
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum MusicTypes : int
          {
              BackgroundMusic = 0,
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum SceneID : int
          {
              Bootstrap = 0,
              MainMenu = 1,
              Game = 2,
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum SettingsTypes : int
          {
              Music = 0,
              Sound = 1,
              Haptic = 2,
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum SoundTypes : int
          {
              UIClick = 0,
              GameWin = 1,
              GameFail = 2,
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene
      {
          public enum UIPanelTypes : int
          {
              LogoHolder = 0,
              LoadingScreen = 1,
              StartPanel = 2,
              InGamePanel = 3,
              GameOverPanel = 4,
              ShopPanel = 5,
              TutorialPanel = 6,
          }
      }
      ```
    * Models & Data: These components, classes, and structs contains the data needed for CrossScene's contents.
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
      {
          [CreateAssetMenu(fileName = "CrossSceneSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/CrossSceneSettings")]
          public sealed class CrossSceneSettings : ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
      {
          public sealed class CrossSceneModel : Model<CrossSceneSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/CrossScene/CrossSceneSettings";
              #endregion
      
              #region Constructor
              public CrossSceneModel() : base(ResourcePath) { }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using Sirenix.OdinInspector;
      using UnityEngine;
      using UnityEngine.Audio;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
      {
          [CreateAssetMenu(fileName = "AudioSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/AudioSettings")]
          public sealed class AudioSettings : SerializedScriptableObject
          {
              #region Fields
              [Header("Audio Settings Fields")]
              [SerializeField] private AudioMixer audioMixer;
              [SerializeField] private Dictionary<MusicTypes, AudioClip> _musics;
              [SerializeField] private Dictionary<SoundTypes, AudioClip> _sounds;
              #endregion
      
              #region Getters
              public AudioMixer AudioMixer => audioMixer;
              public Dictionary<MusicTypes, AudioClip> Musics => _musics;
              public Dictionary<SoundTypes, AudioClip> Sounds => _sounds;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
      using UnityEngine.Audio;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
      {
          public sealed class AudioModel : Model<AudioSettings>, IInitializable
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/CrossScene/AudioSettings";
              private const string AudioPath = "AUDIO_PATH";
              private const string MusicParam = "MUSIC_PARAM";
              private const string SoundParam = "SOUND_PARAM";
              #endregion
      
              #region ReadonlyFields
              private readonly AudioMixer _audioMixer;
              #endregion
      
              #region Fields
              private bool _isSoundMuted;
              private bool _isMusicMuted;
              #endregion
      
              #region Getters
              public bool IsSoundMuted => _isSoundMuted;
              public bool IsMusicMuted => _isMusicMuted;
              #endregion
      
              #region Constructor
              public AudioModel() : base(ResourcePath)
              {
                  _audioMixer = GetSettings.AudioMixer;
      
                  _isMusicMuted = ES3.Load(nameof(_isMusicMuted), AudioPath, false);
                  _isSoundMuted = ES3.Load(nameof(_isSoundMuted), AudioPath, false);
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public void Initialize()
              {
                  SetMusic(_isMusicMuted);
                  SetSound(_isSoundMuted);
              }
              #endregion
      
              #region Executes
              public void SetMusic(bool isActive)
              {
                  _isMusicMuted = isActive;
                  _audioMixer.SetFloat(MusicParam, _isMusicMuted ? -80 : -20);
      
                  Save();
              }
              public void SetSound(bool isActive)
              {
                  _isSoundMuted = isActive;
                  _audioMixer.SetFloat(SoundParam, _isSoundMuted ? -80 : -10);
      
                  Save();
              }
              public void Save()
              {
                  ES3.Save(nameof(_isMusicMuted), _isMusicMuted, AudioPath);
                  ES3.Save(nameof(_isSoundMuted), _isSoundMuted, AudioPath);
              }
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using Sirenix.OdinInspector;
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
      {
          [CreateAssetMenu(fileName = "CurrencySettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/CurrencySettings")]
          public sealed class CurrencySettings : SerializedScriptableObject
          {
              #region Fields
              [Header("Currency Settings Fields")]
              [SerializeField] private Dictionary<CurrencyTypes, int> _defaultCurrencyValues;
              [SerializeField] private Dictionary<CurrencyTypes, Sprite> _currencyIcons;
              #endregion
      
              #region Getters
              public Dictionary<CurrencyTypes, int> DefaultCurrencyValues => _defaultCurrencyValues;
              public Dictionary<CurrencyTypes, Sprite> CurrencyIcons => _currencyIcons;
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
      {
          public sealed class CurrencyModel : Model<CurrencySettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/CrossScene/CurrencySettings";
              private const string CurrencyPath = "CURRENCY_PATH";
              #endregion
      
              #region Fields
              private Dictionary<CurrencyTypes, int> _currencyValues;
              #endregion
      
              #region Getters
              public Dictionary<CurrencyTypes, int> CurrencyValues => _currencyValues;
              #endregion
      
              #region Constructor
              public CurrencyModel() : base(ResourcePath)
              {
                  _currencyValues = ES3.Load(nameof(_currencyValues), CurrencyPath,
                      new Dictionary<CurrencyTypes, int>(GetSettings.DefaultCurrencyValues));
      
                  Save();
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Executes
              public void ChangeCurrencyValue(CurrencyTypes currencyType, int amount, bool isSet)
              {
                  int lasValue = _currencyValues[currencyType];
                  _currencyValues[currencyType] = isSet ? amount : lasValue + amount;
      
                  Save();
              }
              public void Save() => ES3.Save(nameof(_currencyValues), _currencyValues, CurrencyPath);
              #endregion
          }
      }
      ```
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
      {
          [CreateAssetMenu(fileName = "HapticSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/HapticSettings")]
          public sealed class HapticSettings : ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
      {
          public sealed class HapticModel : Model<HapticSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/CrossScene/HapticSettings";
              private const string HapticPath = "HAPTIC_PATH";
              #endregion
      
              #region Fields
              private bool _isHapticMuted;
              #endregion
      
              #region Getters
              public bool IsHapticMuted => _isHapticMuted;
              #endregion
      
              #region Constructor
              public HapticModel() : base(ResourcePath)
              {
                  _isHapticMuted = ES3.Load(nameof(_isHapticMuted), HapticPath, false);
      
                  SetHaptic(_isHapticMuted);
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Executes
              public void SetHaptic(bool isActive)
              {
                  _isHapticMuted = isActive;
                  
                  Save();
              }
              public void Save() => ES3.Save(nameof(_isHapticMuted), _isHapticMuted, HapticPath);
              #endregion
          }
      }
      ```
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
      {
          [CreateAssetMenu(fileName = "LevelSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/LevelSettings")]
          public sealed class LevelSettings : ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
      {
          public sealed class LevelModel : Model<LevelSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/CrossScene/LevelSettings";
              private const string LevelPersistentDataPath = "LEVEL_PERSISTENT_DATA_PATH";
              #endregion
      
              #region Fields
              private LevelPersistentData _levelPersistentData;
              #endregion
      
              #region Getters
              public LevelPersistentData LevelPersistentData => _levelPersistentData;
              #endregion
      
              #region Constructor
              public LevelModel() : base(ResourcePath)
              {
                  _levelPersistentData = ES3.Load(nameof(_levelPersistentData), LevelPersistentDataPath, new LevelPersistentData(0));
      
                  Save();
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region DataExecutes
              public void ResetLevelPersistentData()
              {
                  _levelPersistentData = new(0);
      
                  Save();
              }
              public void UpdateCurrentLevelIndex(bool isSet, int value)
              {
                  _levelPersistentData.CurrentLevelIndex = isSet ? value : _levelPersistentData.CurrentLevelIndex + value;
      
                  Save();
              }
              public void Save() => ES3.Save(nameof(_levelPersistentData), _levelPersistentData, LevelPersistentDataPath);
              #endregion
          }
      }
      ```
    * View & Mediators: These components and classes contains the views and mediation operations of CrossScene's content.
       ```csharp
       using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class AudioView : View
          {
              #region Fields
              [Header("Audio View Fields")]
              [SerializeField] private Dictionary<AudioTypes, AudioSource> _audioSources;
              #endregion
      
              #region Getters
              public Dictionary<AudioTypes, AudioSource> AudioSources => _audioSources;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class AudioMediator : Mediator<AudioView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly AudioModel _audioModel;
              #endregion
      
              #region Constructor
              public AudioMediator(AudioView view, SignalBus signalBus, AudioModel audioModel) : base(view)
              {
                  _signalBus = signalBus;
                  _audioModel = audioModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => _signalBus.Subscribe<PlayAudioSignal>(OnPlayAudio);
              public override void Dispose() => _signalBus.Unsubscribe<PlayAudioSignal>(OnPlayAudio);
              #endregion
      
              #region SignalReceivers
              private void OnPlayAudio(PlayAudioSignal signal) =>
                  PlayAudioProcess(signal.AudioType, signal.MusicType, signal.SoundType);
              #endregion
      
              #region Executes
              private void PlayAudioProcess(AudioTypes audioType, MusicTypes musicType, SoundTypes soundType)
              {
                  switch (audioType)
                  {
                      case AudioTypes.Music:
                          GetView.AudioSources[audioType].clip = _audioModel.GetSettings.Musics[musicType];
                          GetView.AudioSources[audioType].loop = true;
                          GetView.AudioSources[audioType].Play();
                          break;
                      case AudioTypes.Sound:
                          GetView.AudioSources[audioType].PlayOneShot(_audioModel.GetSettings.Sounds[soundType]);
                          break;
                  }
              }
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using TMPro;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class CurrencyView : View
          {
              #region Fields
              [Header("Currency View Fields")]
              [SerializeField] private Dictionary<CurrencyTypes, TextMeshProUGUI> _currencyTexts;
              [SerializeField] private Dictionary<CurrencyTypes, Button> _currencyButtons;
              #endregion
      
              #region Getters
              public Dictionary<CurrencyTypes, TextMeshProUGUI> CurrencyTexts => _currencyTexts;
              public Dictionary<CurrencyTypes, Button> CurrencyButtons => _currencyButtons;
              #endregion
          }
      }
      ```
      ```csharp
      using System.Linq;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Core.Runtime.Utilities.TextFormatter;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Lofelt.NiceVibrations;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
           public sealed class CurrencyMediator : Mediator<CurrencyView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly CurrencyModel _currencyModel;
              #endregion
      
              #region Constructor
              public CurrencyMediator(CurrencyView view, SignalBus signalBus, CurrencyModel currencyModel) : base(view)
              {
                  _signalBus = signalBus;
                  _currencyModel = currencyModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize()
              {
                  RefreshAllCurrencyVisual();
      
                  _signalBus.Subscribe<RefreshCurrencyVisualSignal>(OnRefreshCurrencyVisualSignal);
      
                  GetView.CurrencyButtons.Values.ToList().ForEach(x => x.onClick.AddListener(ButtonClicked));
              }
              public override void Dispose()
              {
                  _signalBus.Unsubscribe<RefreshCurrencyVisualSignal>(OnRefreshCurrencyVisualSignal);
                  
                  GetView.CurrencyButtons.Values.ToList().ForEach(x => x.onClick.RemoveAllListeners());
              }
              #endregion
      
              #region SignalReceivers
              private void OnRefreshCurrencyVisualSignal(RefreshCurrencyVisualSignal signal) =>
                  RefreshCurrencyVisual(signal.CurrencyType);
              #endregion
      
              #region ButtonReceivers
              private void ButtonClicked()
              {
                  _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.ShopPanel));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
      
              #region Executes
              private void RefreshAllCurrencyVisual() =>
                  _currencyModel.CurrencyValues.Keys.ToList().ForEach(RefreshCurrencyVisual);
              private void RefreshCurrencyVisual(CurrencyTypes currencyType)
              {
                  int value = _currencyModel.CurrencyValues[currencyType];
      
                  GetView.CurrencyTexts[currencyType].SetText(TextFormatter.FormatNumber(value));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class LoadingScreenPanelView : View, IUIPanel
          {
              #region Fields
              [Header("Logo Holder Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private Image fillImage;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public Image FillImage => fillImage;
              #endregion
          }
      }
      ```
      ```csharp
      using System;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Cysharp.Threading.Tasks;
      using UnityEngine;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class LoadingScreenPanelMediator : Mediator<LoadingScreenPanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Fields
              private AsyncOperation _loadOperation;
              private bool _loadingCompleted;
              #endregion
      
              #region Constructor
              public LoadingScreenPanelMediator(LoadingScreenPanelView view, SignalBus signalBus) : base(view) =>
                  _signalBus = signalBus;
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize()
              {
                  ChangePanelActivation(false);
      
                  _signalBus.Subscribe<ChangeLoadingScreenActivationSignal>(OnChangeLoadingScreenActivationSignal);
              }
              public override void Dispose() =>
                  _signalBus.Unsubscribe<ChangeLoadingScreenActivationSignal>(OnChangeLoadingScreenActivationSignal);
              #endregion
      
              #region SignalReceivers
              // ReSharper disable once AsyncVoidMethod
              private async void OnChangeLoadingScreenActivationSignal(ChangeLoadingScreenActivationSignal signal)
              {
                  bool isActive = signal.IsActive;
      
                  if (isActive)
                  {
                      ResetProgressBar();
                      
                      _loadOperation = signal.AsyncOperation;
                      
                      ChangePanelActivation(true);
                      WaitUntilSceneIsLoaded();
                  }
                  else
                  {
                      await UniTask.WaitUntil(() => _loadingCompleted);
                      
                      ChangePanelActivation(false);
                  }
              }
              #endregion
      
              #region Executes
              private void ChangePanelActivation(bool isActive)
              {
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isActive);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isActive);
              }
              private void ResetProgressBar()
              {
                  GetView.FillImage.fillAmount = 0f;
                  _loadOperation = null;
                  _loadingCompleted = false;
              }
              private async void WaitUntilSceneIsLoaded()
              {
                  try
                  {
                      while (!_loadOperation.isDone)
                      {
                          float progress = _loadOperation.progress;
                          float targetProgress = _loadOperation.isDone ? 1f : progress;
      
                          // Lerp fill amount towards target progress
                          LerpProgressBar(targetProgress);
      
                          await UniTask.Yield();
                      }
      
                      //async operation finishes at 90%, lerp to full value for a while
                      float time = 0.5f;
                      while (time > 0)
                      {
                          time -= Time.deltaTime;
                          // Lerp fill amount towards target progress
                          LerpProgressBar(1f);
      
                          await UniTask.Yield();
                      }
      
                      await UniTask.Yield();
      
                      _loadingCompleted = true;
                  }
                  catch (Exception e)
                  {
                      Console.WriteLine(e);
                  }
              }
              private void LerpProgressBar(float targetProgress) =>
                  GetView.FillImage.fillAmount = Mathf.Lerp(GetView.FillImage.fillAmount, targetProgress,
                      Time.fixedDeltaTime * 10f);
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class SettingsButtonView: View
          {
              #region Fields
              [Header("Settings Button View Fields")]
              [SerializeField] private GameObject verticalGroup;
              [SerializeField] private Button button;
              [SerializeField] private Button exitButton;
              [SerializeField] private Dictionary<SettingsTypes, Button> _settingsButtons;
              [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOnImages;
              [SerializeField] private Dictionary<SettingsTypes, GameObject> _settingsOffImages;
              #endregion
      
              #region Getters
              public GameObject VerticalGroup => verticalGroup;
              public Button Button => button;
              public Button ExitButton => exitButton;
              public Dictionary<SettingsTypes, Button> SettingsButtons => _settingsButtons;
              public Dictionary<SettingsTypes, GameObject> SettingsOnImages => _settingsOnImages;
              public Dictionary<SettingsTypes, GameObject> SettingsOffImages => _settingsOffImages;
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Application;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Lofelt.NiceVibrations;
      using UnityEngine.SceneManagement;
      using UnityEngine.UI;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class SettingsButtonMediator : Mediator<SettingsButtonView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly AudioModel _audioModel;
              private readonly HapticModel _hapticModel;
              #endregion
      
              #region Fields
              private bool _isVerticalGroupActive;
              #endregion
      
              #region Constructor
              public SettingsButtonMediator(SettingsButtonView view, SignalBus signalBus, AudioModel audioModel, HapticModel hapticModel) : base(view)
              {
                  _signalBus = signalBus;
                  _audioModel = audioModel;
                  _hapticModel = hapticModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize()
              {
                  _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
                  _signalBus.Subscribe<SettingsButtonRefreshSignal>(OnSettingsButtonRefreshSignal);
      
                  GetView.Button.onClick.AddListener(ButtonClicked);
                  GetView.ExitButton.onClick.AddListener(ExitButtonClicked);
      
                  foreach (KeyValuePair<SettingsTypes, Button> item in GetView.SettingsButtons)
                  {
                      SetupVisual(item.Key);
      
                      item.Value.onClick.AddListener(() => SettingsButtonClicked(item.Key));
                  }
              }
              public override void Dispose()
              {
                  _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
                  _signalBus.Unsubscribe<SettingsButtonRefreshSignal>(OnSettingsButtonRefreshSignal);
      
                  GetView.Button.onClick.RemoveListener(ButtonClicked);
                  GetView.ExitButton.onClick.RemoveListener(ExitButtonClicked);
      
                  foreach (KeyValuePair<SettingsTypes, Button> item in GetView.SettingsButtons)
                      item.Value.onClick.RemoveListener(() => SettingsButtonClicked(item.Key));
              }
              #endregion
      
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal) => SetVerticalGroupActivation(false);
              private void OnSettingsButtonRefreshSignal(SettingsButtonRefreshSignal signal) =>
                  SetupVisual(signal.SettingsType);
              #endregion
      
              #region ButtonReceivers
              private void ButtonClicked()
              {
                  SetVerticalGroupActivation(!_isVerticalGroupActive);
      
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              private void ExitButtonClicked()
              {
                  SceneID currentSceneID = (SceneID)SceneManager.GetActiveScene().buildIndex;
      
                  switch (currentSceneID)
                  {
                      case SceneID.Bootstrap:
                          break;
                      case SceneID.MainMenu:
                          _signalBus.Fire(new AppQuitSignal());
                          break;
                      case SceneID.Game:
                          _signalBus.Fire(new GameExitSignal());
                          _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                          _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
                          break;
                  }
              }
              private void SettingsButtonClicked(SettingsTypes settingsType)
              {
                  _signalBus.Fire(new SettingsButtonPressedSignal(settingsType));
      
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
      
              #region Executes
              private void SetupVisual(SettingsTypes settingsType)
              {
                  bool isActive = false;
      
                  switch (settingsType)
                  {
                      case SettingsTypes.Music:
                          isActive = !_audioModel.IsMusicMuted;
                          break;
                      case SettingsTypes.Sound:
                          isActive = !_audioModel.IsSoundMuted;
                          break;
                      case SettingsTypes.Haptic:
                          isActive = !_hapticModel.IsHapticMuted;
                          break;
                  }
      
                  GetView.SettingsOnImages[settingsType].SetActive(isActive);
                  GetView.SettingsOffImages[settingsType].SetActive(!isActive);
              }
              private void SetVerticalGroupActivation(bool isActive)
              {
                  _isVerticalGroupActive = isActive;
                  GetView.VerticalGroup.SetActive(_isVerticalGroupActive);
              }
              #endregion
          }
      }
      ```
      ```csharp
      using TMPro;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class CurrencyTrailView : MonoBehaviour
          {
              #region Fields
              [Header("Currency Trail View Fields")]
              [SerializeField] private Image iconImage;
              [SerializeField] private TextMeshProUGUI amountText;
              #endregion
      
              #region Getters
              public Image IconImage => iconImage;
              public TextMeshProUGUI AmountText => amountText;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using PrimeTween;
      using UnityEngine;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
      {
          public sealed class CurrencyTrailMediator : MonoBehaviour, IPoolable<CurrencyTrailData, IMemoryPool>
          {
              #region Injects
              private SignalBus _signalBus;
              private CurrencyModel _currencyModel;
              private CurrencyTrailView _view;
              #endregion
      
              #region Fields
              private IMemoryPool _memoryPool;
              #endregion
      
              #region PostConstruct
              [Inject]
              private void PostConstruct(SignalBus signalBus, CurrencyModel currencyModel, CurrencyTrailView view)
              {
                  _signalBus = signalBus;
                  _currencyModel = currencyModel;
                  _view = view;
              }
              #endregion
      
              #region Pool
              public void OnSpawned(CurrencyTrailData data, IMemoryPool memoryPool)
              {
                  _memoryPool = memoryPool;
                  
                  SetVisual(data);
                  PlayTween(data);
              }
              public void OnDespawned() => _memoryPool = null;
              #endregion
      
              #region Executes
              private void SetVisual(CurrencyTrailData data)
              {
                  transform.position = data.StartPosition;
                  
                  _view.IconImage.sprite = _currencyModel.GetSettings.CurrencyIcons[data.CurrencyType];
                  _view.IconImage.preserveAspect = true;
                  
                  _view.AmountText.SetText($"{data.Amount}x");
              }
              private void PlayTween(CurrencyTrailData data) =>
                  Tween.Position(transform, data.TargetPosition, data.Duration, data.Ease)
                      .OnComplete(() => TweenCompleteCallback(data));
              private void TweenCompleteCallback(CurrencyTrailData data)
              {
                  _signalBus.Fire(new ChangeCurrencySignal(data.CurrencyType, data.Amount, false));
                  
                  _memoryPool.Despawn(this);
              }
              #endregion
          }
      }
      ```
    * Commands & Signals: These classes and signals provide signal-driven implementation of CrossScene's contents.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
      {
          public sealed class ChangeCurrencyCommand : Command<ChangeCurrencySignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly CurrencyModel _currencyModel;
              #endregion
      
              #region Constructor
              public ChangeCurrencyCommand(SignalBus signalBus, CurrencyModel currencyModel)
              {
                  _signalBus = signalBus;
                  _currencyModel = currencyModel;
              }
              #endregion
      
              #region Executes
              public override void Execute(ChangeCurrencySignal signal)
              {
                  _currencyModel.ChangeCurrencyValue(signal.CurrencyType, signal.Amount, signal.IsSet);
      
                  _signalBus.Fire(new RefreshCurrencyVisualSignal(signal.CurrencyType));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using UnityEngine;
      using UnityEngine.SceneManagement;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
      {
          public sealed class LoadSceneCommand : Command<LoadSceneSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public LoadSceneCommand(SignalBus signalBus) => _signalBus = signalBus;
              #endregion
      
              #region Executes
              public override void Execute(LoadSceneSignal signal)
              {
                  AsyncOperation asyncOperation = SceneManager.LoadSceneAsync((int)signal.SceneId);
      
                  _signalBus.Fire(new ChangeLoadingScreenActivationSignal(true, asyncOperation));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Lofelt.NiceVibrations;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
      {
          public sealed class PlayHapticCommand : Command<PlayHapticSignal>
          {
              #region ReadonlyFields
              private readonly HapticModel _hapticModel;
              #endregion
      
              #region Constructor
              public PlayHapticCommand(HapticModel hapticModel) => _hapticModel = hapticModel;
              #endregion
      
              #region Executes
              public override void Execute(PlayHapticSignal signal)
              {
                  if (_hapticModel.IsHapticMuted)
                      return;
      
                  HapticPatterns.PlayPreset(signal.HapticType);
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.CrossScene
      {
          public sealed class SettingsButtonPressedCommand : Command<SettingsButtonPressedSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly AudioModel _audioModel;
              private readonly HapticModel _hapticModel;
              #endregion
      
              #region Constructor
              public SettingsButtonPressedCommand(SignalBus signalBus, AudioModel audioModel, HapticModel hapticModel)
              {
                  _signalBus = signalBus;
                  _audioModel = audioModel;
                  _hapticModel = hapticModel;
              }
              #endregion
      
              #region Executes
              public override void Execute(SettingsButtonPressedSignal signal)
              {
                  switch (signal.SettingsType)
                  {
                      case SettingsTypes.Music:
                          _audioModel.SetMusic(!_audioModel.IsMusicMuted);
                          break;
                      case SettingsTypes.Sound:
                          _audioModel.SetSound(!_audioModel.IsSoundMuted);
                          break;
                      case SettingsTypes.Haptic:
                          _hapticModel.SetHaptic(!_hapticModel.IsHapticMuted);
                          break;
                  }
      
                  _signalBus.Fire(new SettingsButtonRefreshSignal(signal.SettingsType));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using Lofelt.NiceVibrations;
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene
      {
          public readonly struct ChangeLoadingScreenActivationSignal
          {
              #region ReadonlyFields
              private readonly bool _isActive;
              private readonly AsyncOperation _asyncOperation;
              #endregion
      
              #region Getters
              public bool IsActive => _isActive;
              public AsyncOperation AsyncOperation => _asyncOperation;
              #endregion
      
              #region Constructor
              public ChangeLoadingScreenActivationSignal(bool isActive, AsyncOperation asyncOperation)
              {
                  _isActive = isActive;
                  _asyncOperation = asyncOperation;
              }
              #endregion
          }
          public readonly struct LoadSceneSignal
          {
              #region ReadonlyFields
              private readonly SceneID _sceneId;
              #endregion
      
              #region Getters
              public SceneID SceneId => _sceneId;
              #endregion
      
              #region Constructor
              public LoadSceneSignal(SceneID sceneId) => _sceneId = sceneId;
              #endregion
          } // Has Command
          public readonly struct PlayAudioSignal
          {
              #region ReadonlyFields
              private readonly AudioTypes _audioType;
              private readonly MusicTypes _musicType;
              private readonly SoundTypes _soundType;
              #endregion
      
              #region Getters
              public AudioTypes AudioType => _audioType;
              public MusicTypes MusicType => _musicType;
              public SoundTypes SoundType => _soundType;
              #endregion
      
              #region Constructor
              public PlayAudioSignal(AudioTypes audioType, MusicTypes musicType, SoundTypes soundType)
              {
                  _audioType = audioType;
                  _musicType = musicType;
                  _soundType = soundType;
              }
              #endregion
          }
          public readonly struct PlayHapticSignal
          {
              #region ReadonlyFields
              private readonly HapticPatterns.PresetType _hapticType;
              #endregion
      
              #region Getters
              public HapticPatterns.PresetType HapticType => _hapticType;
              #endregion
      
              #region Constructor
              public PlayHapticSignal(HapticPatterns.PresetType hapticType) => _hapticType = hapticType;
              #endregion
          } // Has Command
          public readonly struct ChangeCurrencySignal
          {
              #region ReadonlyFields
              private readonly CurrencyTypes _currencyType;
              private readonly int _amount;
              private readonly bool _isSet;
              #endregion
      
              #region Getters
              public CurrencyTypes CurrencyType => _currencyType;
              public int Amount => _amount;
              public bool IsSet => _isSet;
              #endregion
      
              #region Constructor
              public ChangeCurrencySignal(CurrencyTypes currencyType, int amount, bool isSet)
              {
                  _currencyType = currencyType;
                  _amount = amount;
                  _isSet = isSet;
              }
              #endregion
          } // Has Command
          public readonly struct SpawnCurrencyTrailSignal
          {
              #region ReadonlyFields
              private readonly CurrencyTrailData _currencyTrailData;
              #endregion
      
              #region Getters
              public CurrencyTrailData CurrencyTrailData => _currencyTrailData;
              #endregion
      
              #region Constructor
              public SpawnCurrencyTrailSignal(CurrencyTrailData currencyTrailData) => _currencyTrailData = currencyTrailData;
              #endregion
          }
          public readonly struct RefreshCurrencyVisualSignal
          {
              #region Fields
              private readonly CurrencyTypes _currencyType;
              #endregion
      
              #region Getters
              public CurrencyTypes CurrencyType => _currencyType;
              #endregion
      
              #region Constructor
              public RefreshCurrencyVisualSignal(CurrencyTypes currencyType) => _currencyType = currencyType;
              #endregion
          }
          public readonly struct SettingsButtonPressedSignal
          {
              #region ReadonlyFields
              private readonly SettingsTypes _settingsType;
              #endregion
      
              #region Getters
              public SettingsTypes SettingsType => _settingsType;
              #endregion
      
              #region Constructor
              public SettingsButtonPressedSignal(SettingsTypes settingsType) => _settingsType = settingsType;
              #endregion
          } // Has Command
          public readonly struct SettingsButtonRefreshSignal
          {
              #region ReadonlyFields
              private readonly SettingsTypes _settingsType;
              #endregion
      
              #region Getters
              public SettingsTypes SettingsType => _settingsType;
              #endregion
      
              #region Constructor
              public SettingsButtonRefreshSignal(SettingsTypes settingsType) => _settingsType = settingsType;
              #endregion
          }
          public readonly struct ChangeUIPanelSignal
          {
              #region ReadonlyFields
              private readonly UIPanelTypes _uiPanelType;
              #endregion
      
              #region Getters
              public UIPanelTypes UIPanelType => _uiPanelType;
              #endregion
      
              #region Constructor
              public ChangeUIPanelSignal(UIPanelTypes uiPanelType) => _uiPanelType = uiPanelType;
              #endregion
          }
      }
      ```
    * Interfaces: This interface is for UIPanels in CrossScene.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene
      {
          public interface IUIPanel
          {
              public UIPanelVo UIPanelVo { get; }
          }
      }
      ```
    * Utilities: This class is for UIPanels in CrossScene.
      ```csharp
      using System;
      using Cysharp.Threading.Tasks;
      using UnityEngine;
      using UnityEngine.Playables;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene
      {
          public static class UIExtensions
          {
              #region UIPanel
              public static void ChangeUIPanelCanvasGroupActivation(this CanvasGroup canvasGroup, bool isActive)
              {
                  canvasGroup.alpha = isActive ? 1 : 0;
                  canvasGroup.interactable = isActive;
                  canvasGroup.blocksRaycasts = isActive;
              }
              public static void ChangeUIPanelTimelineActivation(this PlayableDirector timeline, bool isActive, bool withReversePlay = false, Action reversePlayEnding = null)
              {
                  if (isActive)
                      timeline?.Play();
                  else
                  {
                      if (withReversePlay)
                          TimelineReversePlay(timeline, reversePlayEnding);
                      else
                          timeline?.Stop();
                  }
              }
              #endregion
      
              #region Timeline
              // ReSharper disable once AsyncVoidMethod
              public static async void TimelineReversePlay(this PlayableDirector timeline, Action reversePlayEnding = null)
              {
                  DirectorUpdateMode defaultUpdateMode = timeline.timeUpdateMode;
                  timeline.timeUpdateMode = DirectorUpdateMode.Manual;
      
                  if (timeline.time.ApproxEquals(timeline.duration) || timeline.time.ApproxEquals(0))
                  {
                      timeline.time = timeline.duration;
                  }
                  
                  timeline.Evaluate();
      
                  await UniTask.NextFrame();
      
                  float duration = (float)timeline.duration;
                  while (duration > 0f)
                  {
                      duration -= Time.deltaTime / (float)timeline.duration;
                      timeline.time = Mathf.Max(duration, 0f);
                      timeline.Evaluate();
      
                      await UniTask.NextFrame();
                  }
      
                  timeline.time = 0;
                  timeline.Evaluate();
                  timeline.timeUpdateMode = defaultUpdateMode;
                  timeline.Stop();
                  
                  reversePlayEnding?.Invoke();
              }
              #endregion
      
              #region Math
              public static bool ApproxEquals(this double num, float other) => Mathf.Approximately((float)num, other);
              public static bool ApproxEquals(this double num, double other) => Mathf.Approximately((float)num, (float)other);
              #endregion
          }
      }
      ```
  * Bootstrap: Contains components and classes for SampleGame's splash screen.
    * Installers: These components and classes perform the Zenject bindings required for SampleGame's splash screen.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
      {
          public sealed class BootstrapMonoInstaller : MonoInstaller
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Install<BootstrapModelInstaller>();
                  Container.Install<BootstrapMediationInstaller>();
                  Container.Install<BootstrapSignalInstaller>();
              }
              #endregion
      
              #region Cycle
              public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeBootstrapSignal());
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
      {
          public sealed class BootstrapModelInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings() =>
                  Container.BindInterfacesAndSelfTo<BootstrapModel>().AsSingle().NonLazy();
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
      {
          public sealed class BootstrapMediationInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Bind<LogoHolderPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<LogoHolderPanelMediator>().AsSingle().NonLazy();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Bootstrap;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Bootstrap
      {
          public sealed class BootstrapSignalInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.DeclareSignal<InitializeBootstrapSignal>();
      
                  Container.BindInterfacesAndSelfTo<InitializeBootstrapCommand>().AsSingle().NonLazy();
      
                  Container.BindSignal<InitializeBootstrapSignal>()
                      .ToMethod<InitializeBootstrapCommand>((x, s) => x.Execute(s)).FromResolve();
              }
              #endregion
          }
      }
      ```
    * Models & Data: This component and class contains the data needed for SampleGame's splash screen.
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Bootstrap
      {
          [CreateAssetMenu(fileName = "BootstrapSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/Bootstrap/BootstrapSettings")]
          public sealed class BootstrapSettings : ScriptableObject
          {
              #region Fields
              [Header("Bootstrap Settings Fields")]
              [SerializeField] private Sprite logoSprite;
              [SerializeField] private float sceneActivationDuration;
              #endregion
      
              #region Getters
              public Sprite LogoSprite => logoSprite;
              public float SceneActivationDuration => sceneActivationDuration;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Bootstrap;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap
      {
          public sealed class BootstrapModel : Model<BootstrapSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/Bootstrap/BootstrapSettings";
              #endregion
      
              #region Constructor
              public BootstrapModel() : base(ResourcePath) { }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
          }
      }
      ```
    * View & Mediators: This component and class contains the views and mediation operations of SampleGame's splash screen.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class LogoHolderPanelView : View, IUIPanel
          {
              #region Fields
              [Header("Logo Holder Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private Image logoImage;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public Image LogoImage => logoImage;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Bootstrap
      {
          public sealed class LogoHolderPanelMediator : Mediator<LogoHolderPanelView>
          {
              #region ReadonlyFields
              private readonly BootstrapModel _bootstrapModel;
              #endregion
      
              #region Constructor
              public LogoHolderPanelMediator(LogoHolderPanelView view, BootstrapModel bootstrapModel) : base(view) =>
                  _bootstrapModel = bootstrapModel;
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize()
              {
                  GetView.LogoImage.sprite = _bootstrapModel.GetSettings.LogoSprite;
                  GetView.LogoImage.preserveAspect = true;
      
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(true);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(true);
              }
              public override void Dispose() { }
              #endregion
          }
      }
      ```
    * Commands & Signals: This class and signal is entry point of SampleGame's splash screen.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Bootstrap;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using Cysharp.Threading.Tasks;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Bootstrap
      {
          public sealed class InitializeBootstrapCommand : Command<InitializeBootstrapSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly BootstrapModel _bootstrapModel;
              #endregion
      
              #region Constructor
              public InitializeBootstrapCommand(SignalBus signalBus, BootstrapModel bootstrapModel)
              {
                  _signalBus = signalBus;
                  _bootstrapModel = bootstrapModel;
              }
              #endregion
      
              #region Executes
              // ReSharper disable once AsyncVoidMethod
              public override async void Execute(InitializeBootstrapSignal signal)
              {
                  int millisecondsDelay = (int)(_bootstrapModel.GetSettings.SceneActivationDuration * 1000f);
                  
                  await UniTask.Delay(millisecondsDelay);
      
                  _signalBus.Fire(new LoadSceneSignal(SceneID.MainMenu));
              }
              #endregion
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Bootstrap
      {
          public readonly struct InitializeBootstrapSignal { } // Has Command
      }
      ```
  * MainMenu: Contains the components and classes required for MainMenu operations.
    * Installers: These components and classes perform the Zenject binding required for MainMenu operations.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
      {
          public sealed class MainMenuMonoInstaller : MonoInstaller
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Install<MainMenuModelInstaller>();
                  Container.Install<MainMenuMediationInstaller>();
                  Container.Install<MainMenuSignalInstaller>();
              }
              #endregion
      
              #region Cycle
              public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeMainMenuSignal());
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.MainMenu;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
      {
          public sealed class MainMenuModelInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings() =>
                  Container.BindInterfacesAndSelfTo<MainMenuModel>().AsSingle().NonLazy();
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
      {
          public sealed class MainMenuMediationInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Bind<StartPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.Bind<ShopPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
      
                  Container.BindInterfacesAndSelfTo<StartPanelMediator>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<ShopPanelMediator>().AsSingle().NonLazy();
      
                  Container.Install<ViewMediatorInstaller<CurrencyView, CurrencyMediator>>();
                  Container.Install<ViewMediatorInstaller<SettingsButtonView, SettingsButtonMediator>>();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.MainMenu;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.MainMenu
      {
          public sealed class MainMenuSignalInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.DeclareSignal<InitializeMainMenuSignal>();
      
                  Container.BindInterfacesAndSelfTo<InitializeMainMenuCommand>().AsSingle().NonLazy();
      
                  Container.BindSignal<InitializeMainMenuSignal>().ToMethod<InitializeMainMenuCommand>((x, s) => x.Execute(s))
                      .FromResolve();
              }
              #endregion
          }
      }
      ```
    * Models & Data: This component and class contains the data needed for MainMenu operations.
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.MainMenu
      {
          [CreateAssetMenu(fileName = "MainMenuSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/MainMenu/MainMenuSettings")]
          public sealed class MainMenuSettings: ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.MainMenu;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.MainMenu
      {
          public sealed class MainMenuModel : Model<MainMenuSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/MainMenu/MainMenuSettings";
              #endregion
      
              #region Constructor
              public MainMenuModel() : base(ResourcePath) { }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
          }
      }
      ```
    * View & Mediators: These components and classes contains the views and mediation operations of MainMenu.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using TMPro;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class StartPanelView: View, IUIPanel
          {
              #region Fields
              [Header("Start Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private TextMeshProUGUI levelText;
              [SerializeField] private Button playButton;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public TextMeshProUGUI LevelText => levelText;
              public Button PlayButton => playButton;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Lofelt.NiceVibrations;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
      {
          public sealed class StartPanelMediator: Mediator<StartPanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly LevelModel _levelModel;
              #endregion
      
              #region Constructor
              public StartPanelMediator(StartPanelView view, SignalBus signalBus, LevelModel levelModel) : base(view)
              {
                  _signalBus = signalBus;
                  _levelModel = levelModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => SetCycleSubscriptions(true);
              public override void Dispose() => SetCycleSubscriptions(false);
              #endregion
      
              #region Subscriptions
              private void SetCycleSubscriptions(bool isSub)
              {
                  if (isSub)
                  {
                      _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.PlayButton.onClick.AddListener(OnPlayButtonClicked);
                  }
                  else
                  {
                      _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.PlayButton.onClick.RemoveListener(OnPlayButtonClicked);
                  }
              }
              #endregion
              
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
              {
                  bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
      
                  GetView.PlayButton.interactable = isShow;
      
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
                  
                  if (isShow)
                      SetLevelText();
              }
              #endregion
      
              #region ButtonReceivers
              private void OnPlayButtonClicked()
              {
                  _signalBus.Fire(new LoadSceneSignal(SceneID.Game));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
      
              #region Executes
              private void SetLevelText()
              {
                  int levelNumber = _levelModel.LevelPersistentData.CurrentLevelIndex + 1;
                  GetView.LevelText.SetText($"Level {levelNumber}");
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class ShopPanelView : View, IUIPanel
          {
              #region Fields
              [Header("Shop Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private Button homeButton;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public Button HomeButton => homeButton;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Lofelt.NiceVibrations;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.MainMenu
      {
          public sealed class ShopPanelMediator : Mediator<ShopPanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public ShopPanelMediator(ShopPanelView view, SignalBus signalBus) : base(view) => _signalBus = signalBus;
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => SetCycleSubscriptions(true);
              public override void Dispose() => SetCycleSubscriptions(false);
              #endregion
      
              #region Subscriptions
              private void SetCycleSubscriptions(bool isSub)
              {
                  if (isSub)
                  {
                      _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.HomeButton.onClick.AddListener(OnHomeButtonClicked);
                  }
                  else
                  {
                      _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.HomeButton.onClick.RemoveListener(OnHomeButtonClicked);
                  }
              }
              #endregion
              
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
              {
                  bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
      
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
              }
              #endregion
      
              #region ButtonReceivers
              private void OnHomeButtonClicked()
              {
                  _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.StartPanel));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
          }
      }
      ```
    * Commands & Signals: This class and signal is entry point of MainMenu.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.MainMenu
      {
          public sealed class InitializeMainMenuCommand : Command<InitializeMainMenuSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public InitializeMainMenuCommand(SignalBus signalBus) => _signalBus = signalBus;
              #endregion
      
              #region Executes
              public override void Execute(InitializeMainMenuSignal signal)
              {
                  _signalBus.Fire(new ChangeLoadingScreenActivationSignal(false, null));
                  _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.StartPanel));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Music, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
              }
              #endregion
          }
      }
      ```
      ```csharp
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.MainMenu
      {
          public readonly struct InitializeMainMenuSignal { } // Has Command
      }
      ```
  * Game: Contains the necessary gameplay components and classes for a sample game.
    * Installers: These components and classes perform the Zenject binding required for gameplay.
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
      {
          public sealed class GameMonoInstaller : MonoInstaller
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Install<GameModelInstaller>();
                  Container.Install<GameMediationInstaller>();
                  Container.Install<GameSignalInstaller>();
              }
              #endregion
      
              #region Cycle
              public override void Start() => Container.Resolve<SignalBus>().Fire(new InitializeGameSignal());
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
      {
          public sealed class GameModelInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.BindInterfacesAndSelfTo<GameModel>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<TutorialModel>().AsSingle().NonLazy();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Installers;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
      {
          public sealed class GameMediationInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.Bind<GameOverPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.Bind<InGamePanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
                  Container.Bind<TutorialPanelView>().FromComponentInHierarchy().AsSingle().NonLazy();
      
                  Container.BindInterfacesAndSelfTo<GameOverPanelMediator>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<InGamePanelMediator>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<TutorialPanelMediator>().AsSingle().NonLazy();
      
                  Container.Install<ViewMediatorInstaller<CurrencyView, CurrencyMediator>>();
                  Container.Install<ViewMediatorInstaller<SettingsButtonView, SettingsButtonMediator>>();
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Installers.Game
      {
          public sealed class GameSignalInstaller : Installer
          {
              #region Bindings
              public override void InstallBindings()
              {
                  Container.DeclareSignal<InitializeGameSignal>();
                  Container.DeclareSignal<PlayGameSignal>();
                  Container.DeclareSignal<GameOverSignal>();
                  Container.DeclareSignal<GameExitSignal>();
                  Container.DeclareSignal<SetupGameOverPanelSignal>();
      
                  Container.BindInterfacesAndSelfTo<InitializeGameCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<PlayGameCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<GameOverCommand>().AsSingle().NonLazy();
                  Container.BindInterfacesAndSelfTo<GameExitCommand>().AsSingle().NonLazy();
      
                  Container.BindSignal<InitializeGameSignal>().ToMethod<InitializeGameCommand>((x, s) => x.Execute(s))
                      .FromResolve();
                  Container.BindSignal<PlayGameSignal>().ToMethod<PlayGameCommand>((x, s) => x.Execute(s)).FromResolve();
                  Container.BindSignal<GameOverSignal>().ToMethod<GameOverCommand>((x, s) => x.Execute(s)).FromResolve();
                  Container.BindSignal<GameExitSignal>().ToMethod<GameExitCommand>((x, s) => x.Execute(s)).FromResolve();
              }
              #endregion
          }
      }
      ```
    * Models & Data: These components and classes contains the data needed for gameplay.
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game
      {
          [CreateAssetMenu(fileName = "GameSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/Game/GameSettings")]
          public sealed class GameSettings : ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game
      {
          public sealed class GameModel : Model<GameSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/Game/GameSettings";
              #endregion
      
              #region Constructor
              public GameModel() : base(ResourcePath) { }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
          }
      }
      ```
      ```csharp
      using UnityEngine;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game
      {
          [CreateAssetMenu(fileName = "TutorialSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/Game/TutorialSettings")]
          public sealed class TutorialSettings : ScriptableObject
          {
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.Game;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game
      {
          public sealed class TutorialModel : Model<TutorialSettings>
          {
              #region Constants
              private const string ResourcePath = "Samples/SampleGame/Game/TutorialSettings";
              private const string TutorialPath = "TUTORIAL_PATH";
              #endregion
      
              #region Fields
              private bool _isTutorialShowed;
              #endregion
      
              #region Getters
              public bool IsTutorialShowed => _isTutorialShowed;
              #endregion
      
              #region Constructor
              public TutorialModel() : base(ResourcePath) =>
                  _isTutorialShowed = ES3.Load(nameof(_isTutorialShowed), TutorialPath, false);
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Executes
              public void SetTutorial(bool isActive)
              {
                  _isTutorialShowed = isActive;
                  
                  ES3.Save(nameof(_isTutorialShowed), _isTutorialShowed, TutorialPath);
              }
              #endregion
          }
      }
      ```
    * View & Mediators: These components and classes contains the views and mediation operations of gameplay.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using TMPro;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class InGamePanelView : View, IUIPanel
          {
              #region Fields
              [Header("In Game Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private TextMeshProUGUI levelText;
              [SerializeField] private Transform currencyTrailStartTransform;
              [SerializeField] private Transform currencyTrailTargetTransform;
              [SerializeField] private Button winButton;
              [SerializeField] private Button failButton;
              [SerializeField] private Button addCurrencyButton;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public TextMeshProUGUI LevelText => levelText;
              public Transform CurrencyTrailStartTransform => currencyTrailStartTransform;
              public Transform CurrencyTrailTargetTransform => currencyTrailTargetTransform;
              public Button WinButton => winButton;
              public Button FailButton => failButton;
              public Button AddCurrencyButton => addCurrencyButton;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Lofelt.NiceVibrations;
      using PrimeTween;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          public sealed class InGamePanelMediator : Mediator<InGamePanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly LevelModel _levelModel;
              #endregion
      
              #region Constructor
              public InGamePanelMediator(InGamePanelView view, SignalBus signalBus, LevelModel levelModel) : base(view)
              {
                  _signalBus = signalBus;
                  _levelModel = levelModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => SetCycleSubscriptions(true);
              public override void Dispose() => SetCycleSubscriptions(false);
              #endregion
      
              #region Subscriptions
              private void SetCycleSubscriptions(bool isSub)
              {
                  if (isSub)
                  {
                      _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.WinButton.onClick.AddListener(OnWinButtonPressed);
                      GetView.FailButton.onClick.AddListener(OnFailButtonPressed);
                      GetView.AddCurrencyButton.onClick.AddListener(OnAddCurrencyButtonPressed);
                  }
                  else
                  {
                      _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.WinButton.onClick.RemoveListener(OnWinButtonPressed);
                      GetView.FailButton.onClick.RemoveListener(OnFailButtonPressed);
                      GetView.AddCurrencyButton.onClick.RemoveListener(OnAddCurrencyButtonPressed);
                  }
              }
              #endregion
      
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
              {
                  bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
      
                  if (isShow)
                      SetLevelText();
              }
              #endregion
      
              #region ButtonReceivers
              private void OnWinButtonPressed()
              {
                  _signalBus.Fire(new GameOverSignal(true));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              private void OnFailButtonPressed()
              {
                  _signalBus.Fire(new GameOverSignal(false));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              private void OnAddCurrencyButtonPressed()
              {
                  _signalBus.Fire(new SpawnCurrencyTrailSignal(new CurrencyTrailData(CurrencyTypes.Coin,
                                                                    1,
                                                                    .25f,
                                                                    Ease.Linear,
                                                                    GetView.CurrencyTrailStartTransform.position,
                                                                    GetView.CurrencyTrailTargetTransform.position)));
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
      
              #region Executes
              private void SetLevelText()
              {
                  int levelNumber = _levelModel.LevelPersistentData.CurrentLevelIndex + 1;
                  GetView.LevelText.SetText($"Level {levelNumber}");
              }
              #endregion
          }
      }
      ```
      ```csharp
      using System.Collections.Generic;
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class GameOverPanelView : View, IUIPanel
          {
              #region Fields
              [Header("Game Over Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private Dictionary<bool, GameObject> _gameOverPanels;
              [SerializeField] private Button failHomeButton;
              [SerializeField] private Button successHomeButton;
              [SerializeField] private Button restartButton;
              [SerializeField] private Button nextButton;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public Dictionary<bool, GameObject> GameOverPanels => _gameOverPanels;
              public Button FailHomeButton => failHomeButton;
              public Button SuccessHomeButton => successHomeButton;
              public Button RestartButton => restartButton;
              public Button NextButton => nextButton;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Lofelt.NiceVibrations;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          public sealed class GameOverPanelMediator : Mediator<GameOverPanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public GameOverPanelMediator(GameOverPanelView view, SignalBus signalBus) : base(view) =>
                  _signalBus = signalBus;
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => SetCycleSubscriptions(true);
              public override void Dispose() => SetCycleSubscriptions(false);
              #endregion
      
              #region Subscriptions
              private void SetCycleSubscriptions(bool isSub)
              {
                  if (isSub)
                  {
                      _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
                      _signalBus.Subscribe<SetupGameOverPanelSignal>(OnSetupGameOverPanelSignal);
      
                      GetView.FailHomeButton.onClick.AddListener(OnHomeButtonClicked);
                      GetView.SuccessHomeButton.onClick.AddListener(OnHomeButtonClicked);
                      GetView.RestartButton.onClick.AddListener(OnRestartButtonClicked);
                      GetView.NextButton.onClick.AddListener(OnNextButtonClicked);
                  }
                  else
                  {
                      _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
                      _signalBus.Unsubscribe<SetupGameOverPanelSignal>(OnSetupGameOverPanelSignal);
      
                      GetView.FailHomeButton.onClick.RemoveListener(OnHomeButtonClicked);
                      GetView.SuccessHomeButton.onClick.RemoveListener(OnHomeButtonClicked);
                      GetView.RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
                      GetView.NextButton.onClick.RemoveListener(OnNextButtonClicked);
                  }
              }
              #endregion
      
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
              {
                  bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
              }
              private void OnSetupGameOverPanelSignal(SetupGameOverPanelSignal signal)
              {
                  foreach (var item in GetView.GameOverPanels)
                  {
                      bool isActive = item.Key == signal.IsSuccess;
                      item.Value.SetActive(isActive);
                  }
              }
              #endregion
      
              #region ButtonReceivers
              private void OnHomeButtonClicked()
              {
                  _signalBus.Fire(new GameExitSignal());
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              private void OnRestartButtonClicked()
              {
                  _signalBus.Fire(new PlayGameSignal());
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              private void OnNextButtonClicked()
              {
                  _signalBus.Fire(new PlayGameSignal());
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ValueObjects.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Interfaces.CrossScene;
      using UnityEngine;
      using UnityEngine.UI;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          [RequireComponent(typeof(CanvasGroup))]
          public sealed class TutorialPanelView : View, IUIPanel
          {
              #region Fields
              [Header("Tutorial Panel View Fields")]
              [SerializeField] private UIPanelVo uiPanelVo;
              [SerializeField] private Button closeButton;
              #endregion
      
              #region Getters
              public UIPanelVo UIPanelVo => uiPanelVo;
              public Button CloseButton => closeButton;
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Utilities.CrossScene;
      using Lofelt.NiceVibrations;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.Game
      {
          public sealed class TutorialPanelMediator : Mediator<TutorialPanelView>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly TutorialModel _tutorialModel;
              #endregion
      
              #region Constructor
              public TutorialPanelMediator(TutorialPanelView view, SignalBus signalBus, TutorialModel tutorialModel) : base(view)
              {
                  _signalBus = signalBus;
                  _tutorialModel = tutorialModel;
              }
              #endregion
      
              #region PostConstruct
              public override void PostConstruct() { }
              #endregion
      
              #region Core
              public override void Initialize() => SetCycleSubscriptions(true);
              public override void Dispose() => SetCycleSubscriptions(false);
              #endregion
      
              #region Subscriptions
              private void SetCycleSubscriptions(bool isSub)
              {
                  if (isSub)
                  {
                      _signalBus.Subscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.CloseButton.onClick.AddListener(OnCloseButtonClicked);
                  }
                  else
                  {
                      _signalBus.Unsubscribe<ChangeUIPanelSignal>(OnChangeUIPanelSignal);
      
                      GetView.CloseButton.onClick.RemoveListener(OnCloseButtonClicked);
                  }
              }
              #endregion
      
              #region SignalReceivers
              private void OnChangeUIPanelSignal(ChangeUIPanelSignal signal)
              {
                  bool isShow = signal.UIPanelType == GetView.UIPanelVo.UIPanelType;
      
                  GetView.UIPanelVo.CanvasGroup.ChangeUIPanelCanvasGroupActivation(isShow);
                  GetView.UIPanelVo.PlayableDirector.ChangeUIPanelTimelineActivation(isShow);
              }
              #endregion
      
              #region ButtonReceivers
              private void OnCloseButtonClicked()
              {
                  _tutorialModel.SetTutorial(true);
      
                  _signalBus.Fire(new PlayGameSignal());
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic, SoundTypes.UIClick));
                  _signalBus.Fire(new PlayHapticSignal(HapticPatterns.PresetType.LightImpact));
              }
              #endregion
          }
      }
      ```
    * Commands & Signals: These classes and structures provide entry and exit points to gameplay.
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.Game;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
      {
          public sealed class InitializeGameCommand : Command<InitializeGameSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly TutorialModel _tutorialModel;
              #endregion
      
              #region Constructor
              public InitializeGameCommand(SignalBus signalBus, TutorialModel tutorialModel)
              {
                  _signalBus = signalBus;
                  _tutorialModel = tutorialModel;
              }
              #endregion
      
              #region Executes
              public override void Execute(InitializeGameSignal signal)
              {
                  _signalBus.Fire(new ChangeLoadingScreenActivationSignal(false, null));
      
                  if (_tutorialModel.IsTutorialShowed)
                      _signalBus.Fire(new PlayGameSignal());
                  else
                      _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.TutorialPanel));
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
      {
          public sealed class PlayGameCommand : Command<PlayGameSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public PlayGameCommand(SignalBus signalBus) => _signalBus = signalBus;
              #endregion
      
              #region Executes
              public override void Execute(PlayGameSignal signal) =>
                  _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.InGamePanel));
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
      {
          public sealed class GameOverCommand : Command<GameOverSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              private readonly LevelModel _levelModel;
              #endregion
      
              #region Constructor
              public GameOverCommand(SignalBus signalBus, LevelModel levelModel)
              {
                  _signalBus = signalBus;
                  _levelModel = levelModel;
              }
              #endregion
      
              #region Executes
              public override void Execute(GameOverSignal signal)
              {
                  _signalBus.Fire(new PlayAudioSignal(AudioTypes.Sound, MusicTypes.BackgroundMusic,
                      signal.IsSuccess ? SoundTypes.GameWin : SoundTypes.GameFail));
      
                  _signalBus.Fire(new ChangeUIPanelSignal(UIPanelTypes.GameOverPanel));
                  _signalBus.Fire(new SetupGameOverPanelSignal(signal.IsSuccess));
      
                  _levelModel.UpdateCurrentLevelIndex(false, signal.IsSuccess ? 1 : 0);
              }
              #endregion
          }
      }
      ```
      ```csharp
      using CodeCatGames.HiveMind.Core.Runtime.MVC.Controller;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.CrossScene;
      using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Signals.Game;
      using Zenject;
      
      namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Controllers.Game
      {
          public sealed class GameExitCommand : Command<GameExitSignal>
          {
              #region ReadonlyFields
              private readonly SignalBus _signalBus;
              #endregion
      
              #region Constructor
              public GameExitCommand(SignalBus signalBus) => _signalBus = signalBus;
              #endregion
      
              #region Executes
              public override void Execute(GameExitSignal signal) => _signalBus.Fire(new LoadSceneSignal(SceneID.MainMenu));
              #endregion
          }
      }
      ```

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
