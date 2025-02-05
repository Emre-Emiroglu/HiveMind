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