using CodeCatGames.HiveMind.Core.Runtime.MVC.Model;
using CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Data.ScriptableObjects.CrossScene;
using UnityEngine.Audio;
using Zenject;

namespace CodeCatGames.HiveMind.Samples.Runtime.SampleGame.Models.CrossScene
{
    public sealed class AudioModel : Model<AudioSettings>, IInitializable
    {
        #region Constants
        private const string ResourcePath = "Samples/SampleGame/CrossScene/AudioSettings";
        private const string AudioPath = "AUDIO_PATH";
        private const string MusicParam = "MUSIC_PARAM";
        private const string SoundParam = "SOUND_PARAM";
        #endregion

        #region ReadonlyFields
        private readonly AudioMixer _audioMixer;
        #endregion

        #region Fields
        private bool _isSoundMuted;
        private bool _isMusicMuted;
        #endregion

        #region Getters
        public bool IsSoundMuted => _isSoundMuted;
        public bool IsMusicMuted => _isMusicMuted;
        #endregion

        #region Constructor
        public AudioModel() : base(ResourcePath)
        {
            _audioMixer = GetSettings.AudioMixer;

            _isMusicMuted = ES3.Load(nameof(_isMusicMuted), AudioPath, false);
            _isSoundMuted = ES3.Load(nameof(_isSoundMuted), AudioPath, false);
        }
        #endregion

        #region PostConstruct
        public override void PostConstruct() { }
        #endregion

        #region Core
        public void Initialize()
        {
            SetMusic(_isMusicMuted);
            SetSound(_isSoundMuted);
        }
        #endregion

        #region Executes
        public void SetMusic(bool isActive)
        {
            _isMusicMuted = isActive;
            _audioMixer.SetFloat(MusicParam, _isMusicMuted ? -80 : -20);

            Save();
        }
        public void SetSound(bool isActive)
        {
            _isSoundMuted = isActive;
            _audioMixer.SetFloat(SoundParam, _isSoundMuted ? -80 : -10);

            Save();
        }
        public void Save()
        {
            ES3.Save(nameof(_isMusicMuted), _isMusicMuted, AudioPath);
            ES3.Save(nameof(_isSoundMuted), _isSoundMuted, AudioPath);
        }
        #endregion
    }
}
