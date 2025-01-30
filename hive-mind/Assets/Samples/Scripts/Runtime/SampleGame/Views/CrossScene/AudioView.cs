using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.MVC.View;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Views.CrossScene
{
    public sealed class AudioView : View
    {
        #region Fields
        [Header("Audio View Fields")]
        [SerializeField] private AudioSourcesMap audioSources;
        #endregion

        #region Getters
        public Dictionary<AudioTypes, AudioSource> AudioSources => audioSources.Clone();
        #endregion
    }
    
    [Serializable]
    public sealed class AudioSourcesMap : SerializableDictionaryBase<AudioTypes, AudioSource> { }
}