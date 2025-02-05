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
