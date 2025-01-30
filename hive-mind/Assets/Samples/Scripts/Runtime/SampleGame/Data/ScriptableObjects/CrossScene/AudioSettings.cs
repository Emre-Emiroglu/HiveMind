using System;
using System.Collections.Generic;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/AudioSettings")]
    public sealed class AudioSettings : ScriptableObject
    {
        #region Fields
        [Header("Audio Settings Fields")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private MusicsMap musics;
        [SerializeField] private SoundsMap sounds;
        #endregion

        #region Getters
        public AudioMixer AudioMixer => audioMixer;
        public Dictionary<MusicTypes, AudioClip> Musics => musics.Clone();
        public Dictionary<SoundTypes, AudioClip> Sounds => sounds.Clone();
        #endregion
    }
    
    [Serializable]
    public sealed class MusicsMap : SerializableDictionaryBase<MusicTypes, AudioClip> { }
    [Serializable]
    public sealed class SoundsMap : SerializableDictionaryBase<SoundTypes, AudioClip> { }
}
