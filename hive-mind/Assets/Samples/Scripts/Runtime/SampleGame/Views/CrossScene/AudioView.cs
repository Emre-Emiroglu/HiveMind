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