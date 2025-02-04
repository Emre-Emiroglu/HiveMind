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
  * Exploder: This component manages an explosion effect by detecting physics objects within a specific area, applying an explosion force to scatter them, and optionally restoring them to their original positions.
  * Follower: This component manages a tracking system that allows an object to follow a designated target in various position and rotation spaces, offering different smoothing (lerp) options.
  * Physics: These components detects and handles physical collisions and contacts, managing enter, stay, and exit events for specified objects.
  * Rotator: This component enables continuous rotational movement by rotating objects along a specified axis at a defined speed.
  * SlowMotion: This class manages the activation and deactivation of a slow-motion effect by adjusting the game's timescale.
* MVC: Contains components and classes that enable the use of the MVC architectural pattern together with Zenject.
  * Model: This class contains and encapsulates data. Each model class must contain and encapsulate a ScriptableObject component. The Model class and the ScriptableObject component have a one-to-one relationship. The ScriptableObject component contains and encapsulates non-persistent in-game data.
  * View & Mediator: The View component contains only the components and variables related to the visual appearance of the game object without any operations. The Mediator class provides communication between the View component and the rest of the system. Each mediator class must contain and encapsulate a view component. The Mediator class and the View component have a one-to-one relationship.
  * Command: If the Command class has a signal structure attached to it and this signal is fired, it runs the Execute method. Thanks to the Command class, MVC architectural pattern and signal-oriented development are applied together.
  * ViewMediatorInstaller: It is a Zenject Installer specially developed to ensure that the one-to-one relationship between the Mediator class and the View component can be implemented in cases where there is more than one View component in the game scene.
* ProDebug: Contains classes that provides writing log messages in color and different formats.
  * Colorize: This class is a utility for coloring text in Unity's Debug.Log messages and other text outputs, supporting both RGB and HEX color formats.
  * TextFormat: This class is a utility for formatting text in Unity's Debug.Log messages and other text outputs, allowing text to be bold or italic.
* Utilities: Contains utility classes that are useful and frequently needed in game development processes.
  * Extensions: This class adds extension methods to the Transform component in Unity, enabling axis-based position setting, looking at a target along a specific axis, gradual look direction adjustment, hierarchical object searching, and isometric transformation.
  * FPSDisplay: This component is a Unity component that displays the FPS (frames per second) value on the screen and updates it at set intervals for performance monitoring.
  * TextFormatter: This class is a utility component that manages number and time formatting, providing methods to convert numbers into readable formats and support various time display formats.
  * TimeCalculator: This class provides various time calculation and formatting utility methods to manage local and server-based date/time operations, including computing time differences, adding time, and tracking elapsed time.
  * Utilities: This class provides utility functions for Unity, including tag creation, adjusting CanvasGroup alpha, converting world positions to UI screen positions, shuffling lists, bubble sorting, and performing isometric transformations.

## Samples
* Helpers: Contains components that demonstrating how to use Helpers components and classes.
  * CountdownSample: This component demonstrating how to use the Countdown class.
  * ExploderSample: This component demonstrating how to use the Exploder component.
  * FollowerSample: This component demonstrating how to use the Follower component.
  * PhysicSample: This component demonstrating how to use the Physics components.
  * RotatorSample: This component demonstrating how to use the Rotator component.
  * SlowMotionSample: This component demonstrating how to use the SlowMotion class.
* MVC: Contains components and classes that demonstrating how to use MVC components and classes.
  * Models & Data: These components and classes demonstrating how to use the Model class.
  * Views & Mediators: These components and classes demonstrating how to use the View component and Mediator class.
  * Commands & Signals: These classes and structs demonstrating how to use the Command class and performs the signal-oriented development.
  * Installers: This component demonstrating how to use the ViewMediatorInstaller class and performs the binding operations for Zenject.
* ProDebug: Contains component that demonstrating how to use ProDebug classes.
  * ProDebugSample: This component demonstrating how to use the Colorize and TextFormat classes.
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