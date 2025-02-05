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
  * Models & Data: These components and classes demonstrating how to use the Model class.
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
  * StartupSceneLoader:
  * Application:
    * Installers:
    * Models & Data:
    * Commands & Signals:
  * CrossScene:
    * Installers:
    * Factories:
    * Handlers:
    * Enums:
    * Models & Data:
    * View & Mediators:
    * Commands & Signals:
    * Interfaces:
    * Utilities:
  * Bootstrap:
    * Installers:
    * Models & Data:
    * View & Mediators:
    * Commands & Signals:
  * MainMenu:
    * Installers:
    * Models & Data:
    * View & Mediators:
    * Commands & Signals:
  * Game:
    * Installers:
    * Models & Data:
    * View & Mediators:
    * Commands & Signals:

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
