# Hive Mind

Hive Mind, along with its components, classes and samples, includes Zenject for dependency injection, UniTask for async operations, Prime Tween for tween, Easy Save 3 for data saving and loading operations, Feel/Nice Vibrations for haptics and Sirenix/Odin Inspector for editor. It is a unity project that makes game development processes streamline, scalable, maintainable and modular.

## Features

* Core: It is the core structure of Hive Mind. It contains components and classes to be used in game development processes.

* Samples: Contains examples showing how to use the core structure of Hive Mind.

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

* Helpers:

    * Countdown: It is a countdown class that has the features of pausing, resuming,    adding time, observing whether the countdown is over or not, and having the necessary encapsulates for the countdown.

      Class overview:

        ```
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

    * Exploder:

    * Follower:

    * Physics:

    * Rotator:

    * SlowMotion:

* MVC:

    * Model:

    * View & Mediator:

    * Command:

    * ViewMediatorInstaller:

* Pro Debug:

    * Colorize:

    * TextFormat:

* Utilities:

    * Extensions:

    * FPSDisplay:

    * TextFormatter:

    * TimeCalculator:

## Samples

* Helpers:

    * CountdownSample:
    * ExploderSample:
    * FollowerSample:
    * PhysicSample:
    * RotatorSample:
    * SlowMotionSample:

* MVC:

    * Models & Data:
    * Views & Mediators:
    * Commands:
    * Installers:
    * Signals:

* Pro Debug:

    * ProDebugSample:

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