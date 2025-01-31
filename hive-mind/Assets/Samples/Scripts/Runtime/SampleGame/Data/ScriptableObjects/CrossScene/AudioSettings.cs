using System.Collections.Generic;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Enums.CrossScene;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene
{
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "CodeCatGames/HiveMind/Samples/SampleGame/CrossScene/AudioSettings")]
    public sealed class AudioSettings : SerializedScriptableObject
    {
        #region Fields
        [Header("Audio Settings Fields")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Dictionary<MusicTypes, AudioClip> _musics;
        [SerializeField] private Dictionary<SoundTypes, AudioClip> _sounds;
        #endregion

        #region Getters
        public AudioMixer AudioMixer => audioMixer;
        public Dictionary<MusicTypes, AudioClip> Musics => _musics;
        public Dictionary<SoundTypes, AudioClip> Sounds => _sounds;
        #endregion
    }
}
