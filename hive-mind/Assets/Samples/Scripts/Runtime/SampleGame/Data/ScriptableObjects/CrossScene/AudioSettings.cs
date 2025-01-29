using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Core.Runtime.Utilities.SerializedDictionary;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/AudioSettings")]
    public class AudioSettings : ScriptableObject
    {
        #region Fields
        [Header("Audio Settings Fields")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private MusicsMap musics;
        [SerializeField] private SoundsMap sounds;
        #endregion

        #region Getters
        public AudioMixer AudioMixer => audioMixer;
        public Dictionary<MusicTypes, AudioClip> Musics => musics;
        public Dictionary<SoundTypes, AudioClip> Sounds => sounds;
        #endregion
    }
    
    [Serializable]
    public class MusicsMap : SerializedDictionary<MusicTypes, AudioClip> { }
    [Serializable]
    public class SoundsMap : SerializedDictionary<SoundTypes, AudioClip> { }
}
