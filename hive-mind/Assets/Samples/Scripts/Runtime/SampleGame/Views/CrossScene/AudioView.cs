using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Core.Runtime.Utilities.SerializedDictionary;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public class AudioView : View
    {
        #region Fields
        [Header("Audio View Fields")]
        [SerializeField] private AudioSourcesMap audioSources;
        #endregion

        #region Getters
        public Dictionary<AudioTypes, AudioSource> AudioSources => audioSources;
        #endregion
    }
    
    [Serializable]
    public class AudioSourcesMap : SerializedDictionary<AudioTypes, AudioSource> { }
}